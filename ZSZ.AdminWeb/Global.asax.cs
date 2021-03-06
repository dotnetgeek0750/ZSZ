﻿using Autofac;
using Autofac.Integration.Mvc;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ZSZ.AdminWeb.App_Start;
using ZSZ.AdminWeb.Jobs;
using ZSZ.IService;

namespace ZSZ.AdminWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            log4net.Config.XmlConfigurator.Configure();
           

            ModelBinders.Binders.Add(typeof(string), new TrimToDBCModelBinder());

            #region AOP Filter注册
           
            GlobalFilters.Filters.Add(new JsonNetActionFilter());
            GlobalFilters.Filters.Add(new ZSZAuthorizationFilter());
            GlobalFilters.Filters.Add(new ZSZExceptionFilter());

            #endregion

            #region AutoFac

            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();//把当前程序集中的Controller都注册
            //不要忘了.PropertiesAutowired() 
            //  获取所有相关类库的程序集 
            Assembly[] assemblies = new Assembly[] { Assembly.Load("ZSZ.Service") };
            builder.RegisterAssemblyTypes(assemblies)
            .Where(type => !type.IsAbstract && typeof(IServiceSupport).IsAssignableFrom(type))
            .AsImplementedInterfaces().PropertiesAutowired();

            //typeof(IServiceSupport).IsAssignableFrom(type) 表示IServiceSupport的变量是否可以指向type类型的变量
            //换种说法，type是否实现了IServiceSupport接口 或者 type是否继承了前面的类
            //只注册了实现了IServiceSupport接口的类，避免其他无关的类注册到AutoFac中

            var container = builder.Build();

            //注册系统级别的 DependencyResolver，这样当 MVC 框架创建 Controller 等对象的时候都是管 Autofac要对象。 
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));//!!! 

            #endregion


            StartQuartz();
        }


        private void StartQuartz()
        {
            IScheduler sched = new StdSchedulerFactory().GetScheduler();

            //==============BossReportJob 开始================
            JobDetailImpl jdBossReport = new JobDetailImpl("jdBossReportJob", typeof(BossReportJob));
            IMutableTrigger triggerBossReport = CronScheduleBuilder.DailyAtHourAndMinute(23, 45).Build();//每天23:45执行一次
            triggerBossReport.Key = new TriggerKey("triggerBossReportJob");
            sched.ScheduleJob(jdBossReport, triggerBossReport);
            //==============BossReportJob 结束================


            sched.Start();
        }
    }
}
