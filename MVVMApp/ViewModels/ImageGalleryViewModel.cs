﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using MVVMApp.Helpers;
using MVVMApp.Models;
using MVVMApp.Services;
using MVVMApp.Views;

using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Newtonsoft.Json;

namespace MVVMApp.ViewModels
{
    public class SampleImageWithId
    {
        public string Id;
        public ObservableCollection<SampleImage> Collection;
    }

    public class ImageGalleryViewModel : Observable
    {
        public const string ImageGallerySelectedIdKey = "ImageGallerySelectedIdKey";
        public const string ImageGalleryAnimationOpen = "ImageGallery_AnimationOpen";
        public const string ImageGalleryAnimationClose = "ImageGallery_AnimationClose";

        private NotifyTaskCompletion<ObservableCollection<SampleImage>> _source;
        private ICommand _itemSelectedCommand;
        private GridView _imagesGridView;

        public NotifyTaskCompletion<ObservableCollection<SampleImage>> Source
        {
            get => _source;
            set => Set(ref _source, value);
        }

        public ICommand ItemSelectedCommand => _itemSelectedCommand ?? (_itemSelectedCommand = new RelayCommand<ItemClickEventArgs>(OnsItemSelected));

        public ImageGalleryViewModel()
        {
            // LoadData();
            Source = new NotifyTaskCompletion<ObservableCollection<SampleImage>>(SampleDataService.GetGallerySampleData());
        }

        private async void LoadData()
        {
            // Source = await SampleDataService.GetGallerySampleData();
        }

        public void Initialize(GridView imagesGridView)
        {
            _imagesGridView = imagesGridView;
        }

        public async Task LoadAnimationAsync()
        {
            var selectedImageId = await ApplicationData.Current.LocalSettings.ReadAsync<string>(ImageGallerySelectedIdKey);
            if (!string.IsNullOrEmpty(selectedImageId))
            {
                var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation(ImageGalleryAnimationClose);
                if (animation != null)
                {
                    var item = _imagesGridView.Items.FirstOrDefault(i => ((SampleImage)i).ID == selectedImageId);
                    _imagesGridView.ScrollIntoView(item);
                    await _imagesGridView.TryStartConnectedAnimationAsync(animation, item, "galleryImage");
                }

                ApplicationData.Current.LocalSettings.SaveString(ImageGallerySelectedIdKey, string.Empty);
            }
        }

        private void OnsItemSelected(ItemClickEventArgs args)
        {
            var selected = args.ClickedItem as SampleImage;
            _imagesGridView.PrepareConnectedAnimation(ImageGalleryAnimationOpen, selected, "galleryImage");
            NavigationService.Navigate<ImageGalleryDetailPage>(new SampleImageWithId()
            {
                Id = selected.ID,
                Collection = Source.Result
            });
        }
    }
}
