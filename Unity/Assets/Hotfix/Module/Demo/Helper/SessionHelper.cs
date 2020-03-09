using System;
using ETModel;

namespace ETHotfix
{
    public static class SessionHelper
    {
        public static void HotfixSend(IMessage sendMsg)
        {
            try
            {
                ETHotfix.SessionComponent.Instance.Session.Send(sendMsg);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return;
            }
        }

        public static void ModelSend(ETModel.IMessage sendMsg)
        {
            try
            {
                ETModel.SessionComponent.Instance.Session.Send(sendMsg);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return;
            }
        }

        public static async ETTask<TRes> HotfixCall<TRes>(IRequest requestMsg) where TRes : class, IResponse
        {
            try
            {
                TRes response = await ETHotfix.SessionComponent.Instance.Session.Call(requestMsg) as TRes;
                return response;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public static async ETTask<TRes> ModelCall<TRes>(ETModel.IRequest requestMsg) where TRes : class, ETModel.IResponse
        {
            try
            {
                TRes response = await ETModel.SessionComponent.Instance.Session.Call(requestMsg) as TRes;
                return response;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }
    }
}