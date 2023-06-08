using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcModels;
using Nest;

namespace GrpcElastic
{
    public class TaiKhoanRepository : IESRepository
    {
        #region Init
        protected static string _default_index;

        private static TaiKhoanRepository _instance;

        public TaiKhoanRepository(string modify_index)
        {
            _default_index = !string.IsNullOrEmpty(modify_index) ? modify_index : _default_index;
            ConnectionSettings settings = new ConnectionSettings(connectionPool, sourceSerializer: Nest.JsonNetSerializer.JsonNetSerializer.Default).DefaultIndex(_default_index).DisableDirectStreaming(true);
            settings.MaximumRetries(10);
            client = new ElasticClient(settings);
            var ping = client.Ping(p => p.Pretty(true));
            if (ping.ServerError != null && ping.ServerError.Error != null)
            {
                throw new Exception("START ES FIRST");
            }
        }

        public static TaiKhoanRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    _default_index = prefix_index + "account";
                    _instance = new TaiKhoanRepository(_default_index);
                }
                return _instance;
            }
        }

        public bool CreateIndex(bool delete_if_exist = false, int no_shard = 1, int no_rep = 0)
        {
            if (delete_if_exist && client.Indices.Exists(_default_index).Exists)
                client.Indices.Delete(_default_index);

            var createIndexResponse = client.Indices.Create(_default_index, s => s.Settings(st => st.NumberOfReplicas(no_rep).NumberOfShards(no_shard)).Map<TaiKhoanRepository>(mm => mm.AutoMap().Dynamic(true)));
            return createIndexResponse.IsValid;
        }

        #endregion Init


        #region CRUD
        public bool Index(TaiKhoan account)
        {
            if (account != null)
            {
                return true;
            }
            return false;
        }


        public bool Update(string id, object data)
        {
            bool success = false;

            return success;
        }

        public TaiKhoan GetById(string id, string[] fields = null)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                var res = client.Get<TaiKhoan>(id, s => s.Index(_default_index).SourceIncludes(fields));
                if (res.Found)
                {
                    TaiKhoan acc = res.Source;
                    acc.id = res.Id;
                    return acc;
                }
            }
            return null;
        }
        #endregion
    }
}
