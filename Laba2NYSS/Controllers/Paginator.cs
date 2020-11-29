using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LABA_2.Controller
{
    internal class Paginator<T>
    {
        internal int PagesCount { get; }
        internal int CurrentPageId { get; private set; }
        internal int ItemsPerPage { get; private set; }
        private List<T> _list;
        internal BindingList<T> CurrentPage { get; private set; }

        internal event Action IsLastPage;
        internal event Action IsNotLastPage;
        internal event Action IsFirstPage;
        internal event Action IsNotFirstPage;

        internal Paginator(IEnumerable<T> collection, int itemsPerPage)
        {
            _list = collection.ToList();
            ItemsPerPage = itemsPerPage;
            PagesCount = collection.Count() / ItemsPerPage + 1;
            CurrentPageId = 0;
        }

        internal BindingList<T> PageForward()
        {
            var result = new BindingList<T>();

            if (CurrentPageId < PagesCount - 1)
            {
                for (int i =  ItemsPerPage * CurrentPageId; i < ItemsPerPage * (CurrentPageId + 1); i++)
                {
                    result.Add(_list[i]);
                }
                CurrentPageId++;

                if (CurrentPageId != 1)
                {
                    IsNotFirstPage();
                }
                else
                {
                    IsFirstPage();
                    IsNotLastPage();
                }

                CurrentPage = result;
                return result;
            }
            else
            {
                for (int i = ItemsPerPage * CurrentPageId; i < _list.Count; i++)
                {
                    result.Add(_list[i]);
                }

                CurrentPageId = PagesCount;
                CurrentPage = result;
                IsLastPage();

                return result;
            }
        }

        internal BindingList<T> PageBack()
        {

            if (CurrentPageId == 1)
            {
                return CurrentPage;
            }
            else
            {
                var resList = new List<T>();
                var resBindList = new BindingList<T>();

                for (int i = (CurrentPageId - 1) * ItemsPerPage - 1; i >= (CurrentPageId - 2) * ItemsPerPage; i--)
                {
                    resList.Add(_list[i]);
                }

                for (int i = resList.Count - 1; i >= 0; i--)
                {
                    resBindList.Add(resList[i]);
                }

                CurrentPageId--;

                if (CurrentPageId == 1)
                {
                    IsFirstPage();
                }
                if (CurrentPageId != PagesCount)
                {
                    IsNotLastPage();
                }

                CurrentPage = resBindList;
                return resBindList;
            }
        }

    }
}
