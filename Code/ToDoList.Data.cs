using System;
using System.Collections.Generic;

namespace ToDoList.Data {
    public class Item {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public bool Done { get; set; }
    }
}