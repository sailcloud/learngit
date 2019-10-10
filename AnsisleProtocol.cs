using Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comm;
using Newtonsoft.Json;
using System.Collections;

namespace C0710_CharRoom_Client
{
    public class AnsisleProtocol
    {
        /// <summary>
        /// 连接响应解析
        /// </summary>
        /// <param name="protocol"></param>
        /// <returns></returns>
        public static void ConnectionResponse(ProtocolBean protocol, ref string message, ref bool isSucessed)
        {
            StringBuilder conStr = new StringBuilder();

            ConnResponseModel connModel = JsonConvert.DeserializeObject<ConnResponseModel>(protocol.Payload);
            if (connModel == null)
            {
                conStr.AppendFormat("鉴权失败\r\n");
                conStr.AppendFormat("CmId:{0};MessageType:{1};,Payload:{2}", protocol.CmId, protocol.MsgSeq, protocol.Payload);
            }
            else
            {
                if (connModel.result == 0 || connModel.result == 1)
                {
                    isSucessed = true;
                    conStr.AppendFormat("鉴权成功\r\n");
                }
                else
                {
                    conStr.AppendFormat("鉴权失败\r\n");
                }
                string sn = Commons.DecryptionSN(connModel.sn);
                conStr.AppendFormat("SN:{0}; CmId:{1};MessageType:{2};,Payload:{3}", sn, protocol.CmId, protocol.MsgSeq, protocol.Payload);
            }
            message = conStr.ToString();
        }

        public static void ReportResponse(ProtocolBean protocol, ref string message)
        {
            StringBuilder reportStr = new StringBuilder();
            ReprotResponseModel reportModel = JsonConvert.DeserializeObject<ReprotResponseModel>(protocol.Payload);
            if (reportModel == null || reportModel.result != 0)
            {
                reportStr.Append("上报数据失败\r\n");
            }
            else
            {
                reportStr.Append("上报数据成功\r\n");
            }
            reportStr.AppendFormat("CmId:{0};MsgSeq:{1};Payload:{2}", protocol.CmId, protocol.MsgSeq, protocol.Payload);
            message = reportStr.ToString();
        }

        public static void CmdRequest(ProtocolBean protocol, double latDeg, double latMin, double lonDeg, double lonMin, double totalMileage,
            int satellite, int batter, ref string message, ref byte[] buffer, ref int cmdid)
        {
            try
            {
                StringBuilder cmdStr = new StringBuilder();
                ProtocolCMDReqModel protocolCMDReqModel = JsonConvert.DeserializeObject<ProtocolCMDReqModel>(protocol.Payload);
                if (protocolCMDReqModel == null)
                {
                    cmdStr.Append("命令失败");
                }
                else
                {
                    cmdid = protocolCMDReqModel.cmdID;
                    Hashtable ht = new Hashtable();
                    ht.Add("sn", protocolCMDReqModel.sn);
                    ht.Add("cmdID", cmdid);
                    ht.Add("result", "0");
                    Hashtable subHt = new Hashtable();
                    switch (cmdid)
                    {
                        case 1:
                            message = "开锁成功";
                            Open(latDeg, latMin, lonDeg, lonMin, totalMileage, satellite, batter, ref subHt);
                            ht.Add("data", subHt);
                            break;
                        case 2:
                            message = "上锁成功";
                            Lock(latDeg, latMin, lonDeg, lonMin, totalMileage, satellite, batter, ref subHt);
                            ht.Add("data", subHt);
                            break;
                        case 3:
                            message = "查询成功";
                            Query(latDeg, latMin, lonDeg, lonMin, totalMileage, satellite, batter, ref subHt);
                            ht.Add("data", subHt);
                            break;
                        case 4:
                            message = "响铃成功";
                            break;
                        case 5:
                            message = "闪灯成功";
                            break;
                        case 6:
                            message = "打开电池仓成功";
                            break;
                        case 7: break;
                        case 8: break;
                        case 9: break;
                        case 10: break;
                        case 11: break;
                        case 12: break;
                        case 13: break;
                        case 15:
                            ArgumentModel argument = protocolCMDReqModel.argument;
                            string voiceId = argument != null ? argument.voiceId : string.Empty;
                            message = "报语音成功\r\n VoiceId:" + voiceId;
                            break;

                    }
                    buffer = Comm.ProtocolUtils.encode(6, ht, protocol.MsgSeq);
                }

                message += cmdStr.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void Open(double latDeg, double latMin, double lonDeg, double lonMin, double totalMileage, int satellite, int batter, ref Hashtable subHt)
        {
            subHt.Clear();
            subHt.Add("latitudeDegree", latDeg);
            subHt.Add("latitudeMinute", latMin);
            subHt.Add("longitudeDegree", lonDeg);
            subHt.Add("longitudeMinute", lonMin);
            subHt.Add("totalMileage", totalMileage);
            subHt.Add("battery", batter);
            subHt.Add("satellite", satellite);
        }

        private static void Lock(double latDeg, double latMin, double lonDeg, double lonMin, double totalMileage, int satellite, int batter, ref Hashtable subHt)
        {
            subHt.Add("latitudeDegree", latDeg);
            subHt.Add("latitudeMinute", latMin);
            subHt.Add("longitudeDegree", lonDeg);
            subHt.Add("longitudeMinute", lonMin);
            subHt.Add("totalMileage", totalMileage);
            subHt.Add("battery", batter);
            subHt.Add("satellite", satellite);
        }

        private static void Query(double latDeg, double latMin, double lonDeg, double lonMin, double totalMileage, int satellite, int batter, ref Hashtable subHt)
        {
            subHt.Add("latitudeDegree", latDeg);
            subHt.Add("latitudeMinute", latMin);
            subHt.Add("longitudeDegree", lonDeg);
            subHt.Add("longitudeMinute", lonMin);
            subHt.Add("kickstand", true);
            subHt.Add("satellite", satellite);
            subHt.Add("charging", false);
            subHt.Add("errorCode", "000000000");
            subHt.Add("totalMileage", totalMileage);
            subHt.Add("battery", batter);
        }

    }
}
