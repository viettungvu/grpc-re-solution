using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcModels
{
    public abstract class BaseModel
    {
        public string id { get; set; }
        public string nguoi_sua { get; set; }

        public string nguoi_tao { get; set; }

        public long ngay_sua { get; set; }
        public long ngay_tao { get; set; }
        public List<int> thuoc_tinh { get; set; } = new List<int>();

        public void SetMetadata(string nguoi_sua, bool is_update = false)
        {
            if (is_update)
            {
                this.nguoi_sua = nguoi_sua;
                ngay_sua = XMedia.XUtil.TimeInEpoch(DateTime.Now);
            }
            else
            {
                this.nguoi_sua = nguoi_sua;
                nguoi_tao = nguoi_sua;
                ngay_sua = XMedia.XUtil.TimeInEpoch(DateTime.Now);
                ngay_tao = XMedia.XUtil.TimeInEpoch(DateTime.Now);
            }
        }
    }
}
