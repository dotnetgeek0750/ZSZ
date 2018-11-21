using Autofac;
using Autofac.Integration.Mvc;
using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using ZSZ.IService;

namespace ZSZ.AdminWeb.Jobs
{
    public class BossReportJob : IJob
    {
        private static ILog log = LogManager.GetLogger(typeof(BossReportJob));

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                var sb = new StringBuilder();
                var bossEmail = string.Empty;
                var container = AutofacDependencyResolver.Current.ApplicationContainer;
                using (container.BeginLifetimeScope())
                {
                    var cityService = container.Resolve<ICityService>();
                    var houseService = container.Resolve<IHouseService>();
                    var settingService = container.Resolve<ISettingService>();

                    //从配置表中读取邮箱地址
                    bossEmail = settingService.GetValue("老板邮箱");

                    foreach (var city in cityService.GetAll())
                    {
                        long count = houseService.GetTodayNewHouseCount(city.Id);
                        sb.Append(city.Name).Append("新增房源数量是：").Append(count).AppendLine();
                    }
                }

                using (MailMessage mailMessage = new MailMessage())
                using (SmtpClient smtpClient = new SmtpClient("smpt.163.com"))
                {
                    mailMessage.To.Add(bossEmail);
                    mailMessage.Body = "今日新增房源数量报表";
                    mailMessage.From = new MailAddress("发送邮箱");
                    mailMessage.Subject = "邮件标题";
                    smtpClient.Credentials = new System.Net.NetworkCredential("Smtp发送用户名", "Smtp发送密码");//如果启用了“客户端授权码”，要用授权码代替密码
                    smtpClient.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                log.Error("给老板发报表BossReportJob出错：" + ex);
            }
        }
    }
}