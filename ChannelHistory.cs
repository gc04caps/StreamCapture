using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace StreamCapture
{
    public class ChannelHistory
    {
        Dictionary<string, ChannelHistoryInfo> channelHistoryDict;
        static readonly object _lock = new object();  //used to lock the json load and save portion

        public ChannelHistory()
        {
            try
            {
                lock (_lock)
                {
                    channelHistoryDict = JsonConvert.DeserializeObject<Dictionary<string, ChannelHistoryInfo>>(File.ReadAllText("channelhistory.json"));
                }
            }
            catch(Exception)
            {
                channelHistoryDict = new Dictionary<string, ChannelHistoryInfo>();
            }
        }

        public void Save()
        {
            lock (_lock)
            {
                File.WriteAllText("channelhistory.json", JsonConvert.SerializeObject(channelHistoryDict, Formatting.Indented));
            }
        }

        public ChannelHistoryInfo GetChannelHistoryInfo(string channel)
        {
            ChannelHistoryInfo channelHistoryInfo;

            if(!channelHistoryDict.TryGetValue(channel, out channelHistoryInfo))
            {
                channelHistoryInfo=new ChannelHistoryInfo();
                channelHistoryInfo.channel = channel;
                channelHistoryInfo.hoursRecorded = 0;
                channelHistoryInfo.recordingsAttempted = 0;
                channelHistoryInfo.errors = 0;
                channelHistoryInfo.lastAttempt = DateTime.Now;
                channelHistoryInfo.lastSuccess = DateTime.Now;
                channelHistoryInfo.activeFlag = true;

                channelHistoryDict.Add(channel, channelHistoryInfo);
            }   

            return channelHistoryInfo;
        }
    }
}