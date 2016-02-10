using System;

namespace FeedReader.Parser.Models
{
    public sealed class FeedItem
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Link { get; set; }

        public string Content { get; set; }

        public DateTimeOffset PublishDate { get; set; }
    }
}
