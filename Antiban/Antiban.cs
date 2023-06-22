using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Linq;

namespace Antiban
{
    public class Antiban
    {
        List<AntibanResult> results = new List<AntibanResult>();

        /// <summary>
        /// Добавление сообщений в систему, для обработки порядка сообщений
        /// </summary>
        /// <param name="eventMessage"></param>
        public void PushEventMessage(EventMessage eventMessage)
        {
            var ret = new AntibanResult() { 
                EventMessageId = eventMessage.Id, 
                SentDateTime = eventMessage.DateTime, 
                Phone = eventMessage.Phone,
                Priority = eventMessage.Priority
            };
            if (results.Any(x => x.Phone == eventMessage.Phone))
            {
                var last = results.Where(x => x.Phone == eventMessage.Phone && x.Priority == eventMessage.Priority && eventMessage.Priority == 1).OrderByDescending(x => x.SentDateTime).FirstOrDefault();
                if (last != null)
                {
                    if ((eventMessage.DateTime - last.SentDateTime).TotalHours < 24)
                    {
                        ret.SentDateTime = last.SentDateTime.AddDays(1);
                    }
                }
                else
                {
                    last = results.Where(x => x.Phone == eventMessage.Phone && x.SentDateTime <= ret.SentDateTime).OrderByDescending(x => x.SentDateTime).FirstOrDefault();
                    if ((ret.SentDateTime - last.SentDateTime).TotalMinutes < 1)
                        ret.SentDateTime = last.SentDateTime.AddMinutes(1);
                }
            }
            if (results.Count > 0)
            {
                var last = results.Where(x => x.SentDateTime<ret.SentDateTime.AddSeconds(10)).OrderByDescending(x => x.SentDateTime).FirstOrDefault();
                if((ret.SentDateTime-last.SentDateTime).TotalSeconds < 10)
                {
                    ret.SentDateTime = last.SentDateTime.AddSeconds(10);
                }
            }

            results.Add(ret);
            results = results.OrderBy(x => x.SentDateTime).ToList();

        }

        /// <summary>
        /// Вовзращает порядок отправок сообщений
        /// </summary>
        /// <returns></returns>
        public List<AntibanResult> GetResult()
        {
            return results;
        }
    }
}
