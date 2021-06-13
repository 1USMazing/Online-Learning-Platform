CREATE TABLE [dbo].[Answers](  
    [AnswerID] [int] IDENTITY(1,1) NOT NULL,  
    [AnswerText] [varchar](max) NULL,  
    [QuestionID] [int] NULL,  
 CONSTRAINT [PK_Answers] PRIMARY KEY CLUSTERED   
(  
    [AnswerID] ASC  
))