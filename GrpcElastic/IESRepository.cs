using Elasticsearch.Net;
using Grpc.Models;
using Nest;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text.RegularExpressions;

namespace GrpcElastic
{
    public abstract class IESRepository
    {
        protected static int blockBulk = 500;
        private static string reg_null_or_must_remove = "[\"][a-zA-Z0-9_]*[\"]:null[ ]*[,]?|[\"][a-zA-Z0-9_]*[\"]:" + -int.MaxValue + "[,]?";
        private static string reg_array_null = "\\[( *null *,? *)*]";
        protected static Uri node = new Uri(XMedia.XUtil.ConfigurationManager.AppSetting["ES:Server"]);
        protected static string prefix_index = XMedia.XUtil.ConfigurationManager.AppSetting["ES:PrefixIndex"];
        protected ElasticClient client;
        protected static string formatDatePattern = "yyyy-MM-ddTHH:mm";
        protected static string formatDatePatternEndDay = "yyyy-MM-ddT23:59:59";
        protected static string formatDatePatternStartDay = "yyyy-MM-ddT00:00:00";
        protected static StickyConnectionPool connectionPool = new StickyConnectionPool(new[] { node });
        protected static double maxPriceValue = 3097976931348623;
        protected static uint cacheDate = 43200;
        protected static DateTime dateTime3000 = new DateTime(3000, 1, 1);
        protected static DateMath dateMathNow = DateMath.FromString(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
        protected static DateMath dateMathEndDay = DateMath.FromString(DateTime.Now.ToString("yyyy-MM-ddT23:59:59"));
        protected static string NULL_VALUE = "_null_";
        protected static int max_result_window = 9999;
        public string EscapeTerm(string term)
        {
            string[] rmChars = new string[] { "-", "\"", "+", "=", "&&", "||", ">", "<", "!", "(", ")", "{", "}", "[", "]", "^", "~", ":", "\\", "/" };
            if (term.Count(p => p == '"') % 2 == 0)
            {
                rmChars = new string[] { "-", "+", "=", "&&", "||", ">", "<", "!", "(", ")", "{", "}", "[", "]", "^", "~", ":", "\\", "/" };
            }
            foreach (string item in rmChars)
            {
                term = term.Replace(item, " ");
            }

            return term;
        }

        public string RemoveCharNotAllow(string term)
        {
            string[] rmChars = new string[] { "-", "\"", "+", "=", "&&", "||", ">", "<", "!", "(", ")", "{", "}", "[", "]", "^", "~", ":", "\\", "/", ",", "?", "@", "#", "$", "%", "*", "." };

            foreach (string item in rmChars)
            {
                term = term.Replace(item, " ");
            }

            return term;
        }

        public bool ValidateQuery(string term)
        {
            var vali = new ValidateQueryRequest();
            vali.Query = new QueryContainer(new QueryStringQuery() { Query = term });
            return client.Indices.ValidateQuery(vali).Valid;
        }

        public int GetHashCode(string s)
        {
            int n = s.Length;
            int h = 0;
            for (int i = 0; i < n; i++)
            {
                h += s[i];
                h *= 123123123;
            }
            return h;
        }



        protected bool Index<T>(string _default_index, T data, string route, string id = "") where T : class
        {
            IndexRequest<object> req = new IndexRequest<object>(_default_index, typeof(T));
            if (!string.IsNullOrEmpty(route))
                req.Routing = route;
            req.Document = data;
            IndexResponse re = null;
            if (!string.IsNullOrEmpty(id))
                re = client.Index(data, i => i.Id(id));
            else
                re = client.Index(req);

            try
            {
                Type t = data.GetType();
                PropertyInfo piShared = t.GetProperty("id");
                if (piShared != null)
                    piShared.SetValue(data, re.Id);
            }
            catch (Exception ex)
            { }
            return re.Result == Result.Created;
        }

        protected bool Index<T>(string _default_index, T data, string route, out string id) where T : class
        {
            id = "";
            IndexRequest<object> req = new IndexRequest<object>(_default_index, typeof(T));
            if (!string.IsNullOrEmpty(route))
                req.Routing = route;
            req.Document = data;
            IndexResponse re = null;

            re = client.Index(req);
            if (re.Result == Result.Created)
                id = re.Id;
            return re.Result == Result.Created;
        }

        protected bool Update<T>(string _default_index, string id, object doc) where T : class
        {
            try
            {
                var res = client.Update<T, object>(id, u => u.Index(_default_index).Doc(doc));
                return res.Result == Result.Updated || res.Result == Result.Noop;
            }
            catch (Exception)
            {
            }
            return false;

        }


        protected bool IndexMany<T>(string _default_index, IEnumerable<T> docs, out IEnumerable<string> success_ids) where T : class
        {
            bool success = false;
            success_ids = new List<string>();
            try
            {
                var res = client.Bulk(bu => bu.Index(_default_index).IndexMany(docs));
                success = res.IsValid && !res.Errors;
                success_ids = res.Items.Where(x => x.IsValid).Select(x => x.Id);
            }
            catch (Exception)
            {


            }
            return success;
        }

        protected bool UpdateMany<T>(string _default_index, IEnumerable<object> docs, out IEnumerable<string> error_ids) where T : class
        {
            bool success = false;
            error_ids = new List<string>();
            try
            {
                var res = client.Bulk(bu => bu.Index(_default_index).UpdateMany(docs, (b, doc) => b.Doc(doc)));
                success = res.IsValid && !res.Errors;
                foreach (var item in res.ItemsWithErrors)
                {
                    error_ids.Append(item.Id);
                }
            }
            catch (Exception) { }
            return success;
        }

        protected bool Refresh(string _DefaultIndex)
        {
            var re = client.Indices.Refresh(_DefaultIndex, r => r.RequestConfiguration(c => c.RequestTimeout(TimeSpan.FromSeconds(5))));
            return re.IsValid;
        }

        public async Task<ConcurrentBag<IHit<T>>> GetObjectScrollAsync<T>(string _default_index, QueryContainer query, SourceFilter so, string scroll_timeout = "5m", int scroll_pageize = 2000) where T : class
        {
            if (query == null)
                query = new MatchAllQuery();
            if (so == null)
                so = new SourceFilter() { };
            ConcurrentBag<IHit<T>> bag = new ConcurrentBag<IHit<T>>();
            try
            {
                var searchResponse = await client.SearchAsync<T>(sd => sd.Source(s => so).Index(_default_index).From(0).Take(scroll_pageize).Query(q => query).Scroll(scroll_timeout));

                while (true)
                {
                    if (!searchResponse.IsValid || string.IsNullOrEmpty(searchResponse.ScrollId))
                        break;

                    if (!searchResponse.Documents.Any())
                        break;

                    var tmp = searchResponse.Hits;
                    foreach (var item in tmp)
                    {
                        bag.Add(item);
                    }
                    searchResponse = await client.ScrollAsync<T>(scroll_timeout, searchResponse.ScrollId);
                }

                await client.ClearScrollAsync(new ClearScrollRequest(searchResponse.ScrollId));
            }
            catch (Exception)
            {
            }
            finally
            {
            }
            return bag;
        }

        public ConcurrentBag<IHit<T>> GetObjectScroll<T>(string _default_index, QueryContainer query, SourceFilter so, string scroll_timeout = "5m", int scroll_pageize = 2000) where T : class
        {
            if (query == null)
                query = new MatchAllQuery();
            if (so == null)
                so = new SourceFilter() { };
            ConcurrentBag<IHit<T>> bag = new ConcurrentBag<IHit<T>>();
            try
            {
                ISearchResponse<T> searchResponse;
                if (so != null && so.Includes != null && so.Includes.Any())
                    searchResponse = client.Search<T>(sd => sd.Source(s => so).Index(_default_index).From(0).Take(scroll_pageize).Query(q => query).Scroll(scroll_timeout));
                else
                    searchResponse = client.Search<T>(sd => sd.Index(_default_index).From(0).Take(scroll_pageize).Query(q => query).Scroll(scroll_timeout));
                while (true)
                {
                    if (!searchResponse.IsValid || string.IsNullOrEmpty(searchResponse.ScrollId))
                        break;

                    if (!searchResponse.Documents.Any())
                        break;

                    var tmp = searchResponse.Hits;
                    foreach (var item in tmp)
                    {
                        bag.Add(item);
                    }
                    searchResponse = client.Scroll<T>(scroll_timeout, searchResponse.ScrollId);
                }

                client.ClearScroll(new ClearScrollRequest(searchResponse.ScrollId));
            }
            catch (Exception)
            {
            }
            finally
            {
            }
            return bag;
        }

        protected T HitToDocument<T>(IMultiGetHit<T> hit) where T : class, new()
        {
            if (hit.Found)
            {
                var doc = hit.Source;

                Type t = doc.GetType();
                PropertyInfo piShared = t.GetProperty("id");
                piShared.SetValue(doc, hit.Id);
                return doc;
            }
            return new T();
        }

        protected T HitToDocument<T>(IHit<T> hit) where T : class
        {
            var doc = hit.Source;
            Type t = doc.GetType();
            PropertyInfo piShared = t.GetProperty("id");
            if (piShared != null)
                piShared.SetValue(doc, hit.Id);
            return doc;
        }

        protected T GetById<T>(string _default_index, string id, string[] view_field = null) where T : class
        {
            try
            {
                var g = new GetRequest(_default_index, id);
                if (view_field != null && view_field.Length > 0 && !view_field.Contains("null"))
                {
                    g.SourceIncludes = view_field;
                }
                var re = client.Get<T>(g);
                if (re.Found)
                    return re.Source;
            }
            catch (Exception)
            {
            }
            return null;
        }

        public T GetById<T>(string id, string[] view_fields = null) where T : class
        {
            GetResponse<T> re = null;

            try
            {
                if (view_fields == null || view_fields.Contains("*"))
                {
                    re = client.Get<T>(id);
                }
                else
                {
                    re = client.Get<T>(id, g => g.SourceIncludes(view_fields));
                }
                if (re != null && re.Found)
                {
                    var doc = re.Source;

                    Type t = doc.GetType();
                    PropertyInfo piShared = t.GetProperty("id");
                    if (piShared != null)
                        piShared.SetValue(doc, re.Id);
                    return doc;
                }
            }
            catch
            {
            }
            return default;
        }

        public bool DeleteById<T>(string id) where T : class
        {
            var re = client.Delete<T>(id);
            return re.Result == Result.Deleted;
        }

        protected IEnumerable<T> GetAll<T>(string _default_index, string term, string[] search_fields, string[] view_fields, int page, int page_size, out long total_recs, List<QueryContainer> must_add = null) where T : class
        {
            total_recs = 0;

            List<QueryContainer> must = new List<QueryContainer>();
            if (must_add != null)
            {
                must.AddRange(must_add);
            }
            if (!string.IsNullOrEmpty(term))
            {
                must.Add(new QueryStringQuery()
                {
                    Fields = search_fields,
                    Query = term
                });
            }
            else
                must.Add(new MatchAllQuery());
            QueryContainer queryFilterMultikey = new QueryContainer(new BoolQuery()
            {
                Must = must
            });

            SourceFilter soF = new SourceFilter() { Includes = view_fields };
            if (view_fields.Contains("*"))
            {
                soF = new SourceFilter();
            }

            List<ISort> sort = new List<ISort>()
            {
            };
            if (!string.IsNullOrEmpty(term))
                sort.Add(new FieldSort() { Field = "_score", Order = SortOrder.Descending });
            SearchRequest request = new SearchRequest(Indices.Index(_default_index))
            {
                TrackTotalHits = true,
                Query = queryFilterMultikey,
                Source = soF,
                Sort = sort,
                Size = page_size,
                From = (page - 1) * page_size
            };
            var re = client.Search<T>(request);

            total_recs = re.Total;

            var lst = re.Hits.Select(x => HitToDocument(x));
            return lst;
        }

        protected bool Update<T>(string _default_index, T data, string id) where T : class
        {
            ConnectionSettings settings_update = new ConnectionSettings(connectionPool, sourceSerializer: Nest.JsonNetSerializer.JsonNetSerializer.Default).DefaultIndex(_default_index).DisableDirectStreaming(true);
            var client = new ElasticClient(settings_update);

            string json_doc = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            Regex regex = new Regex(reg_null_or_must_remove);
            json_doc = regex.Replace(json_doc, string.Empty);
            regex = new Regex(reg_array_null);
            json_doc = regex.Replace(json_doc, "[]");
            //json_doc = json_doc.Replace("\"ngay_tao\":0,", "").Replace("\"auto_id\":0,", "");
            var ob_up = Newtonsoft.Json.JsonConvert.DeserializeObject(json_doc);
            var re_up = client.Update<T, object>(id, u => u.Doc(ob_up));

            if (re_up.Result == Result.Error)
            {
                return Index(_default_index, data, "", id);
            }
            return re_up.Result == Result.Updated || re_up.Result == Result.Noop;
        }


        public bool UpdateThuocTinh<T>(string id, List<int> thuoc_tinh) where T : class
        {
            var re = client.Update<T, object>(id, u => u.Doc(new { thuoc_tinh }));
            return re.Result == Result.Updated || re.Result == Result.Noop;
        }

        protected bool UpdateByQuery<T>(string _default_index, string term, string field, string up_script) where T : class
        {
            var res = client.UpdateByQuery<T>(up => up.Index(_default_index).Query(q => q.Term(t => t.Field(field).Value(term))).Script(up_script));
            return res.IsValid;
        }


        public bool UpdateMany(IEnumerable<object> list_object, string index, out List<string> id_error)
        {
            id_error = new List<string>();
            var res = client.Bulk(b => b.Index(index).UpdateMany(list_object, (bu, d) => bu.Doc(d)));
            if (res.ItemsWithErrors.Any())
            {
                id_error = new List<string>();
                foreach (var item in res.ItemsWithErrors)
                {
                    id_error.Add(item.Id);
                }
            }
            return res.IsValid && !res.Errors && !res.ItemsWithErrors.Any();
        }


        public bool UpdateCommon<T>(string default_index, string id, object data) where T : class
        {
            var res = client.Update<T, object>(id, u => u.Index(default_index).Doc(data));
            return res.IsValid && (res.Result == Result.Updated || res.Result == Result.Noop);
        }


        public bool DeleteMany<T>(string default_index, IEnumerable<string> ids) where T : class
        {
            DeleteByQueryRequest del_req = new DeleteByQueryRequest(default_index)
            {
                Query = new QueryContainer(new IdsQuery { Values = ids.Select(x => (Id)x) })
            };
            var res = client.DeleteByQuery(del_req);
            return res.IsValid && res.Deleted == ids.Count();
        }

        /// <summary>
        /// Tạo sort default trên field ngay_sua cho search request
        /// </summary>
        protected IList<ISort> DefaultSort()
        {
            return new List<ISort>()
            {
                new FieldSort{Field="ngay_sua", Order=SortOrder.Descending}
            };
        }


        /// <summary>
        /// Tạo custom sort cho request theo dic_sort, trường hợp sử dụng sort default sẽ sort theo ngày sửa mới nhất
        /// </summary>
        protected IList<ISort> XMCustomSort(Dictionary<string, IOrder> dic_sort, bool use_sort_default = false)
        {
            var sort_lst = new List<ISort>();

            if (dic_sort != null && dic_sort.Any())
            {
                foreach (var item in dic_sort)
                {
                    sort_lst.Add(new FieldSort { Field = item.Key, Order = item.Value == IOrder.ASC ? SortOrder.Ascending : SortOrder.Descending });
                }
            }
            if (use_sort_default)
            {
                sort_lst.Add(new FieldSort { Field = "ngay_sua", Order = SortOrder.Descending });
            }
            return sort_lst;
        }

        /// <summary>
        /// Tạo custom filter cho thuộc tính search
        /// </summary>
        protected List<QueryContainer> XMCustomThuocTinhSearch(List<QueryContainer> filter, IEnumerable<int> thuoc_tinh, bool and_opeator_thuoc_tinh = true)
        {
            if (filter != null)
            {
                if (thuoc_tinh != null && thuoc_tinh.Any())
                {
                    if (and_opeator_thuoc_tinh)
                    {
                        foreach (int tt in thuoc_tinh)
                        {
                            filter.Add(new TermQuery { Field = "thuoc_tinh", Value = tt });
                        }
                    }
                    else
                    {
                        filter.Add(new TermsQuery { Field = "thuoc_tinh", Terms = thuoc_tinh.Select(tt => (object)tt) });
                    }
                }
            }
            return filter;
        }



        protected SourceFilter XMCustomSource(string[] view_fields, string[] ignore_fields = null)
        {
            if (view_fields == null || view_fields.Contains("*"))
            {
                if (ignore_fields != null)
                {
                    return new SourceFilter { Includes = "*", Excludes = ignore_fields };
                }
                return new SourceFilter { Includes = "*" };
            }
            else
            {
                return new SourceFilter { Includes = view_fields };
            };
        }



        protected SearchRequest XMSetSize(SearchRequest request, int page, int page_size)
        {
            if (page < 1) page = 1;
            if (page_size < 0) page_size = 10;
            request.From = (page - 1) * page_size;
            request.Size = page_size;
            return request;
        }
    }


    public static class IESExtension
    {
        public static void XMSetSize(this SearchRequest request, int page, int page_size)
        {
            if (page < 1) page = 1;
            if (page_size < 0) page_size = 10;
            request.From = (page - 1) * page_size;
            request.Size = page_size;
        }
    }
}
