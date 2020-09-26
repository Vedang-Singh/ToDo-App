using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ToDoList.Data;
using ToDoList.Shared;

namespace ToDoList.Pages
{
    public partial class ToDoApp
    {
        [Inject] private IJSRuntime Js { get; set; }
        [Inject] private Blazored.LocalStorage.ISyncLocalStorageService LocalStore { get; set; }
        public List<Item> Todos = new List<Item>();
        private string inpVal = "";
        private string desc = "";
        private string listTitle;
        public const string Key = "list_data_json";
        public const string TitleKey = "list_title";

        public void AddTodo()
        {
            if (!string.IsNullOrWhiteSpace(inpVal) && !string.IsNullOrWhiteSpace(desc))
            {
                var itm = new Item
                {
                    Title = inpVal,
                    Desc = desc,
                    Id = Guid.NewGuid().ToString(),
                    Done = false
                };
                Todos.Add(itm);
                UpdateLocalStorage();
            }
            else
            {
                Js.InvokeVoidAsync("alert", "Title and/or Description cannot be empty.");
            }
        }

        public void UpdateLocalStorage()
        {
            LocalStore.SetItem(Key, Todos);
        }

        public void UpdateTitle(string val)
        {
            listTitle = val;
            LocalStore.SetItem(TitleKey, val);
        }

        public void ClearLocalStorage()
        {
            Todos = new List<Item>();
            UpdateLocalStorage();
        }

        protected override void OnInitialized()
        {
            listTitle = LocalStore.GetItem<string>(TitleKey) ?? "My Tasks";
            Todos = LocalStore.GetItem<List<Item>>(Key) ?? new List<Item>();
            UpdateLocalStorage();
        }
    }
}