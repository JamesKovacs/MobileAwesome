﻿using Autofac;
using MobileAwesomeApp.Infrastructure.Mongo;
using MobileAwesomeApp.Services;
using MongoDB.Driver;

namespace MobileAwesomeApp.Infrastructure.Autofac
{
    public class MobileAwesomeAppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            // Register lower modules
            // builder.RegisterModule<...>();

            builder
                .RegisterTypes(
                    typeof(HardcodedSettingsProvider),
                    typeof(RestaurantService))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<MongoClientFactory>().AsSelf(); // TODO: interface?
            builder.Register<IMongoClient>(
                (context) =>
                {
                    var clientFactory = context.Resolve<MongoClientFactory>();
                    var client = clientFactory.CreateMongoClient();
                    return client;
                })
                .SingleInstance();
            builder.RegisterType<MongoClientWrapper>().AsSelf().SingleInstance();
        }
    }
}