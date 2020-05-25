using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PlaceImageDemo
{
    public class Cat
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Hobby { get; set; }
        public Uri ImageUri { get; set; }
    }

    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "Redundant")]
    public class Menagerie : List<Cat>
    {
        public Menagerie()
        {
            Add(new Cat { Name = "Felix", Age = 4, Hobby = "Sleeping", ImageUri = new Uri("http://placekitten.com/150/100") });
            Add(new Cat { Name = "Mittens", Age = 3, Hobby = "Napping", ImageUri = new Uri("http://placekitten.com/100/150") });
            Add(new Cat { Name = "Toonces", Age = 8, Hobby = "Snoozing", ImageUri = new Uri("http://placekitten.com/100/100") });
        }
    }
}
