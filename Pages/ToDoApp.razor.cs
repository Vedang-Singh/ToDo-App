using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ToDoList.Data;
using ToDoList.Config;
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
        ElementReference inpRef;
        ElementReference decRef;


        public void AddTodo()
        {
            if (!string.IsNullOrWhiteSpace(inpVal))
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
                Js.InvokeVoidAsync("clearInput", inpRef, decRef);
            }
            else
            {
                Js.InvokeVoidAsync("alert", "Title and/or Description cannot be empty.");
            }
        }

        public void UpdateLocalStorage()
        {
            LocalStore.SetItem(Keys.ListKey, Todos);
        }

        public void ClearLocalStorage()
        {
            Todos = new List<Item>();
            UpdateLocalStorage();
        }

        protected override void OnInitialized()
        {
            listTitle = LocalStore.GetItem<string>(Keys.TitleKey) ?? "My Tasks";
            Todos = LocalStore.GetItem<List<Item>>(Keys.ListKey) ?? new List<Item>();
            UpdateLocalStorage();
        }

        public void UpdateTitle(string val)
        {
            listTitle = val;
            LocalStore.SetItem(Keys.TitleKey, val);
        }

        public void ClearCompleted()
        {
            var filtered = 
                from item in Todos
                where !item.Done 
                select item;
            
            Todos = filtered.ToList();
            UpdateLocalStorage();
        }
    }
}