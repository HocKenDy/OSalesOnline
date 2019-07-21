using Erp.BackOffice.Crm.Models;
using Erp.Domain.Entities;
using System.Web.Mvc;

namespace Erp.BackOffice.Crm
{
    public class CrmAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Crm";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                 "Crm_Campaign",
                 "Campaign/{action}/{id}",
                 new { controller = "Campaign", action = "Index", id = UrlParameter.Optional }
             );

            context.MapRoute(
                 "Crm_Process",
                 "Process/{action}/{id}",
                 new { controller = "Process", action = "Index", id = UrlParameter.Optional }
             );

            context.MapRoute(
                "Crm_ProcessAction",
                "ProcessAction/{action}/{id}",
                new { controller = "ProcessAction", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
            "Crm_Task",
            "Task/{action}/{id}",
            new { controller = "Task", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
            "Crm_ProcessStage",
            "ProcessStage/{action}/{id}",
            new { controller = "ProcessStage", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
            "Crm_ProcessStep",
            "ProcessStep/{action}/{id}",
            new { controller = "ProcessStep", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
          "Crm_Report",
          "CrmReport/{action}/{id}",
          new { controller = "CrmReport", action = "Index", id = UrlParameter.Optional }
          );
            //context.MapRoute(
            //"Crm_Question",
            //"Question/{action}/{id}",
            //new { controller = "Question", action = "Index", id = UrlParameter.Optional }
            //);

            //context.MapRoute(
            //"Crm_Answer",
            //"Answer/{action}/{id}",
            //new { controller = "Answer", action = "Index", id = UrlParameter.Optional }
            //);

            //context.MapRoute(
            //"Crm_Vote",
            //"Vote/{action}/{id}",
            //new { controller = "Vote", action = "Index", id = UrlParameter.Optional }
            //);

            context.MapRoute(
            "Crm_ProcessApplied",
            "ProcessApplied/{action}/{id}",
            new { controller = "ProcessApplied", action = "Index", id = UrlParameter.Optional }
            );

            //context.MapRoute(
            //"Crm_SMSLog",
            //"SMSLog/{action}/{id}",
            //new { controller = "SMSLog", action = "Index", id = UrlParameter.Optional }
            //);

            //context.MapRoute(
            //"Crm_EmailLog",
            //"EmailLog/{action}/{id}",
            //new { controller = "EmailLog", action = "Index", id = UrlParameter.Optional }
            //);

            context.MapRoute(
          "Crm_Post",
          "Post/{action}/{id}",
          new { controller = "Post", action = "Index", id = UrlParameter.Optional }
          );
            context.MapRoute("Crm_Note",
            "Note/{action}/{id}",
            new { controller = "Note", action = "Index", id = UrlParameter.Optional }
            );

            //<append_content_route_here>

            RegisterAutoMapperMap();
        }

        private static void RegisterAutoMapperMap()
        {
            AutoMapper.Mapper.CreateMap<Domain.Crm.Entities.Campaign, CampaignViewModel>();
            AutoMapper.Mapper.CreateMap<CampaignViewModel, Domain.Crm.Entities.Campaign>();

            AutoMapper.Mapper.CreateMap<Domain.Crm.Entities.Process, ProcessViewModel>();
            AutoMapper.Mapper.CreateMap<ProcessViewModel, Domain.Crm.Entities.Process>();
            AutoMapper.Mapper.CreateMap<Domain.Crm.Entities.ProcessAction, ProcessActionViewModel>();
            AutoMapper.Mapper.CreateMap<ProcessActionViewModel, Domain.Crm.Entities.ProcessAction>();

            AutoMapper.Mapper.CreateMap<Domain.Crm.Entities.Task, TaskViewModel>();
            AutoMapper.Mapper.CreateMap<TaskViewModel, Domain.Crm.Entities.Task>();
            AutoMapper.Mapper.CreateMap<TaskTemplateViewModel, Domain.Crm.Entities.Task>();
            AutoMapper.Mapper.CreateMap<Domain.Crm.Entities.vwTask, TaskViewModel>();
            AutoMapper.Mapper.CreateMap<TaskViewModel, Domain.Crm.Entities.vwTask>();

            AutoMapper.Mapper.CreateMap<Domain.Crm.Entities.ProcessStage, ProcessStageViewModel>();
            AutoMapper.Mapper.CreateMap<ProcessStageViewModel, Domain.Crm.Entities.ProcessStage>();

            AutoMapper.Mapper.CreateMap<Domain.Crm.Entities.ProcessStep, ProcessStepViewModel>();
            AutoMapper.Mapper.CreateMap<ProcessStepViewModel, Domain.Crm.Entities.ProcessStep>();

            //AutoMapper.Mapper.CreateMap<Domain.Crm.Entities.Question, QuestionViewModel>();
            //AutoMapper.Mapper.CreateMap<QuestionViewModel, Domain.Crm.Entities.Question>();

            //AutoMapper.Mapper.CreateMap<Domain.Crm.Entities.Answer, AnswerViewModel>();
            //AutoMapper.Mapper.CreateMap<AnswerViewModel, Domain.Crm.Entities.Answer>();

            AutoMapper.Mapper.CreateMap<Domain.Crm.Entities.ProcessApplied, ProcessAppliedViewModel>();
            AutoMapper.Mapper.CreateMap<ProcessAppliedViewModel, Domain.Crm.Entities.ProcessApplied>();

            AutoMapper.Mapper.CreateMap<Domain.Crm.Entities.Post, PostViewModel>();
            AutoMapper.Mapper.CreateMap<PostViewModel, Domain.Crm.Entities.Post>();

            AutoMapper.Mapper.CreateMap<Domain.Crm.Entities.vwPost, PostViewModel>();
            AutoMapper.Mapper.CreateMap<PostViewModel, Domain.Crm.Entities.vwPost>();
            

            //AutoMapper.Mapper.CreateMap<qts.webapp.backend.domain.Models.Crm.Note, NoteViewModel>();
            //AutoMapper.Mapper.CreateMap<NoteViewModel, qts.webapp.backend.domain.Models.Crm.Note>();


            //<append_content_mapper_here>
        }
    }
}
