using System;

namespace PackingTracker.Common
{
    /// <summary>
    /// Description of Constants.
    /// </summary>
    public sealed class Constants
    {
        /// <summary>
        /// 内箱
        /// </summary>
        public static string BoxTypeInner = "inner";

        /// <summary>
        /// 设备外箱
        /// </summary>
        public static string BoxTypeOuterForDev = "outerfordev";

        /// <summary>
        /// 外箱
        /// </summary>
        public static string BoxTypeOuterForBox = "outerforbox";

        private Constants()
        {
        }
#if DEBUG
        public static string Host = "https://lifecycle2.ibroadlink.com";
#else
		public static string Host = "https://lifecyle2.ibroadlink.com";
#endif

        public static string GetError(int errCode)
        {
            string msg = "错误码：" + errCode + ", ";
            switch (errCode)
            {
                case -5000:
                    msg += "不支持的协议版本";
                    break;
                case -5001:
                    msg += "服务器忙";
                    break;
                case -5002:
                    msg += "数据错误";
                    break;
                case -5003:
                    msg += "不是有效的申请";
                    break;
                case -5004:
                    msg += "请求时间过期";
                    break;
                case -5005:
                    msg += "没有权限";
                    break;
                case -5006:
                    msg += "mongo索引冲突";
                    break;
                case -5007:
                    msg += "没有该设备";
                    break;
                case -5008:
                    msg += "设备已被烧录过";
                    break;
                case -5009:
                    msg += "http method 错误";
                    break;
                case -5010:
                    msg += "工厂已存在";
                    break;
                case -5011:
                    msg += "工厂申请已存在";
                    break;
                case -5012:
                    msg += "尚未创建没有管理组，不能进行此项操作";
                    break;
                case -5013:
                    msg += "没有权限";
                    break;
                case -5014:
                    msg += "mac地址分配冲突";
                    break;
                case -5015:
                    msg += "设备处理错误的状态";
                    break;
                case -5016:
                    msg += "上传步骤非法";
                    break;
                case -5017:
                    msg += "MAC不属于该代工厂";
                    break;
                case -5018:
                    msg += "MAC重复绑定";
                    break;

                case -5019:
                    msg += "查询不到记录";
                    break;

                case -5020:
                    msg += "工单不存在";
                    break;

                case -5021:
                    msg += "申请容量超出工单限制";
                    break;

                case -5022:
                    msg += "箱体类型不匹配";
                    break;

                case -5023:
                    msg += "箱体已装满";
                    break;

                case -5024:
                    msg += "已绑定";
                    break;

                case -5025:
                    msg += "箱体无法容纳";
                    break;

                case -5026:
                    msg += "查询数量过多";
                    break;

                case -5027:
                    msg += "箱体未装满";
                    break;

                case -5028:
                    msg += "操作不允许,请检查输入";
                    break;

                case -5029:
                    msg += "设备不存在";
                    break;

                case -5030:
                    msg += "设备状态不符";
                    break;

                case -5031:
                    msg += "设备不属于工厂";
                    break;

                case -5032:
                    msg += "工单不存在";
                    break;

                case -5033:
                    msg += "箱体不存在";
                    break;

                case -5034:
                    msg += "未绑定";
                    break;

                case -5035:
                    msg += "上报数据出错";
                    break;

                case -5036:
                    msg += "单次绑定数量过多";
                    break;

                case -5037:
                    msg += "存在设备状态不符";
                    break;

                case -5038:
                    msg += "设备在不同的箱体";
                    break;

                case -5039:
                    msg += "箱体在不同的箱体";
                    break;

                case -5040:
                    msg += "未绑定同一外箱";
                    break;

                case -5041:
                    msg += "存在已绑定";
                    break;

                case -5042:
                    msg += "存在内箱未装箱完成";
                    break;

                case -5043:
                    msg += "未找到相关信息";
                    break;

                case -5044:
                    msg += "设备箱体不属于同一工单";
                    break;

                case -5045:
                    msg += "已存在";
                    break;

                case -5046:
                    msg += "工单已经生成，无法修改订单";
                    break;

                case -5047:
                    msg += "订单已经生成";
                    break;

                case -5048:
                    msg += "订单不存在";
                    break;

                case -5049:
                    msg += "没有输入工厂id";
                    break;

                case -5050:
                    msg += "没有输入userid";
                    break;

                case -5051:
                    msg += "产品不存在";
                    break;
                case -5052:
                    msg += "订单不属于该工厂";
                    break;
                case -5053:
                    msg += "工单数目和订单不符合";
                    break;
                case -5054:
                    msg += "数据不存在记录";
                    break;
                case -5055:
                    msg += "没有产品id";
                    break;
                case -5056:
                    msg += "没有工单id";
                    break;
                case -5057:
                    msg += "订单只能申请一个工单";
                    break;
                case -5058:
                    msg += "没有sn前缀";
                    break;
                case -5059:
                    msg += "工单不存在";
                    break;
                case -5060:
                    msg += "输入错误";
                    break;
                case -5061:
                    msg += "MAC不属于该代工厂";
                    break;
                case -5062:
                    msg += "Mac和sn不属于同一个工单";
                    break;
                case -5064:
                    msg += "工单未生成sn";
                    break;
                case -5065:
                    msg += "Mac不属于工单";
                    break;
                case -5066:
                    msg += "资料不存在";
                    break;
                case -5067:
                    msg += "资料类型错误，支持struct,package,hardware,firmware";
                    break;
                case -5068:
                    msg += "重复上报该步骤";
                    break;
                case -5069:
                    msg += "之前上报出错，需要从头开始";
                    break;
                case -5070:
                    msg += "上报步骤顺序出错";
                    break;
                case -5071:
                    msg += "前后步骤总数不一致";
                    break;
                case -5072:
                    msg += "License不存在";
                    break;
                case -5073:
                    msg += "类型错误";
                    break;
                case -5074:
                    msg += "已经装箱，请首先取消装箱";
                    break;
                case -5075:
                    msg += "订单不属于该工厂";
                    break;
                case -5076:
                    msg += "厂商不允许，请联系古北";
                    break;
                case -5077:
                    msg += "主模块已绑定";
                    break;
                case -5078:
                    msg += "orderId不存在";
                    break;
                case -5079:
                    msg += "processindex非法，应该小于processcount";
                    break;
                case -5080:
                    msg += "整机检测上报失败";
                    break;
                case -5081:
                    msg += "没有sn";
                    break;
                case -5082:
                    msg += "没有did";
                    break;
                case -5083:
                    msg += "did,sn太多";
                    break;
                case -5084:
                    msg += "sn不属于订单或工厂";
                    break;
                case -5085:
                    msg += "设备不属于订单";
                    break;
                case -5113:
                    msg += "该内箱已输入其他外箱";
                    break;
                case -5130:
                    msg += "获取AP信息失败";
                    break;
                case -9002:
                    msg += "权限中心错误";
                    break;
                default:
                    msg += "未知错误";
                    break;
            }
            return msg;
        }
    }
}
