using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using MVVMApp.Models;

namespace MVVMApp.Services
{
    public static class SampleDataService
    {
        public static ObservableCollection<SampleImage> GetGallerySampleData()
        {
            var data = new ObservableCollection<SampleImage>();
            for (int i = 1; i <= 10; i++)
            {
                data.Add(new SampleImage()
                {
                    ID = $"{i}",
                    Source = $"ms-appx:///Assets/SampleData/SamplePhoto{i}.png",
                    Name = $"Image sample {i}"
                });
            }

            return data;
        }
    }
}
