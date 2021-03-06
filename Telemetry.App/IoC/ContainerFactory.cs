﻿using Autofac;
using Telemetry.App.Aplication;
using Telemetry.App.Application.Interfaces;
using Telemetry.App.Repository;
using Telemetry.App.Repository.Interfaces;
using Telemetry.App.Utils;
using Telemetry.App.Utils.Interfaces;

namespace Telemetry.App.IoC
{
	public class ContainerFactory
	{
		public IContainer Create()
		{
			var builder = new ContainerBuilder();

			builder.RegisterType<ConnectionHandler>().As<IConnectionHandler>();
			builder.RegisterType<Initializer>().As<IInitializer>();
			builder.RegisterType<LogWritter>().As<ILogWritter>();
			builder.RegisterType<RecordHandler>().As<IRecordHandler>();
			builder.RegisterType<TcpSocketClient>().As<ITcpSocketClient>();
			builder.RegisterType<FloatRounder>().As<IFloatRounder>();

			return builder.Build();
		}
	}
}
