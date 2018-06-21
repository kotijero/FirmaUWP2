using Firma.Bl;
using Firma.Helpers.DataVirtualization;
using Firma.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml.Data;

namespace ViewModel.CustomDataSources
{
    public class DokumentDataSource : INotifyCollectionChanged, System.Collections.IList, IItemsRangeInfo
    {
        // koristi se za slanje naredbi UI thread-u
        private CoreDispatcher _dispatcher;

        private ItemCacheManager<Dokument> itemCache;

        private int _count = 1;

        private List<int> idList;

        BlDokument BlDokument;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private DokumentDataSource(BlDokument blDokument, List<int> idList, TypedEventHandler<object, CacheChangedEventArgs<Dokument>> action)
        {
            this.BlDokument = blDokument;
            this.idList = idList;
            _dispatcher = Windows.UI.Xaml.Window.Current.Dispatcher;
            _count = idList.Count;

            this.itemCache = new ItemCacheManager<Dokument>(fetchDataCallback, 50);
            this.itemCache.CacheChanged += ItemCache_CacheChanged;
            this.itemCache.CacheChanged += action;
        }

        public static async Task<DokumentDataSource> GetDataSourceAsync(BlDokument blDokument, TypedEventHandler<object, CacheChangedEventArgs<Dokument>> action)
        {
            List<int> idList = await Task.Run(() => blDokument.FetchAllIds());
            return new DokumentDataSource(blDokument, idList, action);
        }

        #region IList Implementation

        public bool Contains(object value)
        {
            return IndexOf(value) != -1;
        }

        public int IndexOf(object value)
        {
            return (value != null) ? itemCache.IndexOf((Dokument)value) : -1;
        }

        public object this[int index]
        {
            get
            {
                // Ako u cache nema zapisa, vratiti ce null. Jednom kad je dohvacen,
                // zvati ce changed event da dojavi kontroli liste
                return itemCache[index];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Count
        {
            get { return _count; }
        }

        #endregion

        // Postoji zbog zahtjeva sucelja IItemsRangeInfo .
        public void Dispose()
        {
            itemCache = null;
        }

        /// <summary>
        /// Primarna metoda za sucelje IItemsRangeInfo.
        /// Zove se kada se listi promijeni pogled.
        /// </summary>
        /// <param name="visibleRange">The range of items that are actually visible</param>
        /// <param name="trackedItems">Additional set of ranges that the list is using, for example the buffer regions and focussed element</param>
        public void RangesChanged(ItemIndexRange visibleRange, IReadOnlyList<ItemIndexRange> trackedItems)
        {
            // znamo da će vidljivi rang biti unutar pracenih zapisa, zato ga ne trebamo predati
            // dodavanje novog seta rangova. Ako će biti potrebno, zvati će callback za nove podatke
            itemCache.UpdateRanges(trackedItems.ToArray());
        }


        private async Task<Dokument[]> fetchDataCallback(ItemIndexRange batch, CancellationToken ct)
        {
            List<Dokument> fetched = new List<Dokument>();
            for(int i = batch.FirstIndex; i <= batch.LastIndex; i++)
            {
                // Ako se odustalo od zahtjeva, prekida se dohvat.
                ct.ThrowIfCancellationRequested();

                ResultWrapper<Dokument> response = null;
                await Task.Run(() => response = BlDokument.FetchWithoutStavke(idList[i], true));
                if (!string.IsNullOrEmpty(response.ErrorMessage))
                {
                    fetched.Add(default(Dokument));
                }
                else
                {
                    fetched.Add(response.Value);
                }
            }
            return fetched.ToArray();
        }

        // Ovo se okine kada se zapis doda u cache
        private void ItemCache_CacheChanged(object sender, CacheChangedEventArgs<Dokument> args)
        {
            
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, args.oldItem, args.newItem, args.itemIndex));
        }

        #region Djelovi IList koji nisu implementirani

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }
        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
