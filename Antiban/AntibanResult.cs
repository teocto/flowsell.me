﻿using System;

namespace Antiban
{
    public class AntibanResult
    {
        /// <summary>
        /// Предполагаемое время отправки сообщения
        /// </summary>
        public DateTime SentDateTime { get; set; }
        public int EventMessageId { get; set; }
        public string Phone { get; set; }
        public int Priority { get; set; }
    }
}
