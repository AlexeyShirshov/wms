<HandleError()> _
Public Class HomeController
    Inherits System.Web.Mvc.Controller

    Function Index() As ActionResult
        ViewData("Message") = "Welcome to ASP.NET MVC!"
        ViewData("TypesCount") = Config.WmsRoot.Config.Domains(0).Model.GetTypes.Count()
        Return View()
    End Function

    Function About() As ActionResult
        Return View()
    End Function
End Class
