using System.Web.Mvc;

namespace BHIC.Portal.Areas.QuestionEngine
{
    public class QuestionEngineAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "QuestionEngine";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute
            (
                "QuestionEngine_default",
                "QuestionEngine/{controller}/{action}/{id}",
                new { controller = "MyQuestions", action = "Index", id = UrlParameter.Optional }
                , namespaces: new[] { "BHIC.Portal.Areas.QuestionEngine.Controllers" }
            );

            context.MapRoute
            (
                "QuestionEngine_default2",
                "QuestionEngine/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
                , namespaces: new[] { "BHIC.Portal.Areas.QuestionEngine.Controllers" }
            );
        }
    }
}