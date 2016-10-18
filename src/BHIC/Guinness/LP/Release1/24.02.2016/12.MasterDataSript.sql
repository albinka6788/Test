
-- MASTER DATA RECORDS INSERTION SCRIPTS
EXEC [dbo].[InsertTemplates] "Template1.cshtml","/LandingPage/Images/Logo/logo.png"
EXEC [dbo].[InsertTemplates] "Template2.cshtml","/LandingPage/Images/Logo/logo.png"
EXEC [dbo].[InsertTemplates] "Template3.cshtml","/LandingPage/Images/Logo/logo.png"
EXEC [dbo].[InsertTemplates] "Template4.cshtml","/LandingPage/Images/Logo/logo.png"
EXEC [dbo].[InsertTemplates] "DummyTemplate.cshtml","/LandingPage/Images/Logo/Hydrangeas.jpg"

INSERT INTO dbo.tblLandingPageUsers(Id,Username,[Password]) VALUES (1,'admin','admin')
INSERT INTO dbo.tblLandingPageUsers(Id,Username,[Password]) VALUES (2,'adc','adc')