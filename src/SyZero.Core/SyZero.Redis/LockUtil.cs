using FreeRedis;
using System;
using System.Threading;
using System.Threading.Tasks;
using SyZero.Util;

namespace SyZero.Redis
{
    public class LockUtil : ILockUtil
    {
        private readonly RedisClient _redis;
        //默认过期时间10s
        private readonly int _defaultExpires = 10;
        //等待时重试时间间隔：毫秒，默认60毫秒
        private readonly int _retryInterval = 60;
        //1970年
        private readonly DateTime _time1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        public LockUtil(RedisClient cach)
        {
            _redis = cach;
        }

        public bool GetLock(string lockKey, int expiresSenconds = 10, int waitTimeSenconds = 10)
        {

            if (expiresSenconds <= 0)
            {
                expiresSenconds = _defaultExpires;
            }

            if (waitTimeSenconds <= 0)
            {
                return _redis.SetNx(lockKey, 1, expiresSenconds);
            }

            long now = CurrentTimeStamp();
            long waitEndTime = now + waitTimeSenconds * 1000;
            bool result = false;

            while (!result && now <= waitEndTime)
            {
                result = _redis.SetNx(lockKey, 1, expiresSenconds);

                if (!result)
                {
                    Thread.Sleep(_retryInterval);
                    now = CurrentTimeStamp();
                }
            }

            return result;
        }

        public async Task<bool> GetLockAsync(string lockKey, int expiresSenconds = 10, int waitTimeSenconds = 10)
        {
            if (expiresSenconds <= 0)
            {
                expiresSenconds = _defaultExpires;
            }

            if (waitTimeSenconds <= 0)
            {
                return  _redis.SetNx(lockKey, 1, expiresSenconds);
            }

            long now = CurrentTimeStamp();
            long waitEndTime = now + waitTimeSenconds * 1000;
            long leftTime = 0;
            bool result = false;

            while (!result && now <= waitEndTime)
            {
                result = _redis.SetNx(lockKey, 1, expiresSenconds);

                if (!result)
                {
                    leftTime = waitEndTime - now;

                    if (leftTime >= _retryInterval)
                    {
                        await Task.Delay(_retryInterval);
                    }
                    else
                    {
                        await Task.Delay((int)leftTime);
                    }

                    now = CurrentTimeStamp();
                }
            }

            return result;
        }

        public void Release(string lockKey)
        {
            _redis.Del(lockKey);
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        private long CurrentTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - _time1970;
            return Convert.ToInt64(ts.TotalSeconds * 1000);
        }
    }
}
