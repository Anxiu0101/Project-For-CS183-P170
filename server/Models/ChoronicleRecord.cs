using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using server.Data;

namespace server.Models
{
    /// <summary>
    /// Representing the type of the record. Showing the data source
    /// of the record.
    /// </summary>
    /// <remarks>
    /// This type told you what the data source is. It's not the
    /// real type, but since the programer want to operate different
    /// data in seperated ways, this attribute was named as 'type'
    /// </remarks>
    public enum ChronicleRecordType
    {
        /// <summary>
        /// The data came from ... what the hell?
        /// </summary>
        Unkown = -1,
        /// <summary>
        /// For searching unification. DO NOT using it as a real type!
        /// </summary>
        All = -2,
        /// <summary>
        /// The data came from zhihu(zhihu.com)
        /// </summary>
        Zhihu,
        /// <summary>
        /// The data came from weibo(weibo.com)
        /// </summary>
        Weibo
    }

    /// <summary>
    /// Chronicle records, representing the data fetched
    /// from websites.
    /// </summary>
    public class ChronicleRecord
    {
        /// <summary>
        /// Record ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// When this recored was recored.
        /// </summary>
        [Display(Name = "Recorded Time")]
        [DataType(DataType.DateTime)]
        public DateTime RecordedTime { get; set; }

        /// <summary>
        /// The type of the record.
        /// </summary>
        /// <remarks>
        /// Representing this record's data source. <br />
        /// See also: <seealso cref="ChronicleRecordType" />
        /// </remarks>
        [Display(Name = "Data Source")]
        public ChronicleRecordType Type { get; set; }

        public List<TopicEntry> Topics{ get; set; }
    }
}