﻿using System.Configuration;
using System.Data;
using System.Windows;

namespace WpfBasicApp02
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly Bootstrapper _bootstrapper;

        public App()
        {
            _bootstrapper = new Bootstrapper();
        }
    }

}
