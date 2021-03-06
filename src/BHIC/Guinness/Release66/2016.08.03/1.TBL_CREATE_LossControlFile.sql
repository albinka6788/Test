
CREATE TABLE [LossControlFiles]
(
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [varchar](50) NULL,
	[FileName] [varchar](500) NULL,
	[Language] [varchar](10) NULL,
	[IsDownloadable] [bit] NULL,
 CONSTRAINT [PK_LossControlFiles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [LossControlFiles] ON 

GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (1, N'9aa6e119-de03-43b5-b3fa-bb33428e0c2d', N'101_Stress_Relievers.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (2, N'11b3662e-0b12-4883-9a26-04bcb8766e43', N'Accidents_Cost_Everyone.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (3, N'b4883946-8f6f-406c-8134-e2701b3f3618', N'Accidents_dont_just_happen.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (4, N'c2b3d811-6669-4a6f-ab8e-7d71e3704a65', N'Alcohol_and_Drugs_Have_No_Place_in_the_Workplace.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (5, N'3181842a-c30d-4b17-8a24-2ee1019023f7', N'Alcohol_and_Other_Drugs_Affect_Safety.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (6, N'619f4af3-7ea2-4638-98dc-85c605181e04', N'Assess_Your_Stress.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (7, N'460addf5-6b71-4e77-84ec-2f798675e9c0', N'Attitude_Is_Everything.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (8, N'cec99465-7b6a-44ab-a653-93d41c3a785f', N'A_Guide_to_Worksite_Fire_Safety.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (9, N'ecf94a60-c87a-4075-98d7-61c1f4dd3af4', N'Back_Exercises--Making_Your_Back_Work_for_You.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (10, N'724504d2-9183-4378-a49f-5c8286958235', N'Back_Exercises.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (11, N'c638ab9b-0333-417b-98f3-3a9fe5de673c', N'Back_Stretches.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (12, N'42dfdf5f-8435-42dd-9600-c2166aece0b2', N'Bad_wires_can_cause_fires.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (13, N'd28d63d5-8b8c-494a-a697-31ece98d3a20', N'Basic_Rules_for_Forklift_Operation.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (14, N'bc9fce2c-4672-42f0-8fc5-180997f8d828', N'Basic_Rules_for_Hand_Tools.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (15, N'f0646ee6-e63e-4e95-a384-6c74cf9cda3c', N'Basic_Rules_for_Portable_Power_Tools.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (16, N'a362d9b4-13dc-465f-a5ea-d8e68bfc4959', N'Basic_rule_of_thumb--Leave_guards_in_place.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (17, N'528d3de9-9fce-460e-97a9-0d30af8d684a', N'Before_you_work_on_it--lock_it_out.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (18, N'63fb4754-c7e5-44ab-8603-be1670015327', N'Bloodborne_Pathogens--HIV_and_Hepatitis_B.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (19, N'd3b612a9-352a-4d87-a826-ef690072c8ee', N'Bloodborne_Pathogens--Universal_Precautions.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (20, N'7111309a-e20f-47ba-84d4-09bfad1a7dd3', N'Blood_Alcohol_Concentration_and_You.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (21, N'db14604b-b8fc-436d-81b1-fb491c6279b0', N'But_they_were_just_horsing_around.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (22, N'84fd2b68-ca45-4803-8e03-f3fd120d2d14', N'Call_for_help_before_you_try_to_put_it_out.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (23, N'c79badc1-0fd9-4d36-acbd-2c8497cfcc32', N'Characteristics_of_a_Defensive_Driver.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (24, N'8d36f6de-2005-4b80-ac66-b286351a536e', N'Choosing_and_Using_Eye_Protection.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (25, N'ecc48ffe-b33b-44f3-8f06-fbbed277e63a', N'Choosing_and_Using_Hearing_Protection.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (26, N'cefb3d21-60e7-4bc4-980b-aaf0283bc833', N'Choosing_and_Using_Protective_Headwear.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (27, N'146acf8e-dcfb-4bcc-a29b-bbce3aed4bc1', N'Choosing_and_Using_Respiratory_Protection.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (28, N'89cd1490-5412-4007-9c4b-f090cad8f1ae', N'Choosing_and_Using_Work_Gloves.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (29, N'5e31d0ad-b856-40a6-9d09-1498b8f08046', N'Choosing_and_Using_Work_Shoes.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (30, N'0ee437be-96b8-4dab-81cb-1f4ab2559d53', N'Cleaning_the_Right_Way.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (31, N'64633405-a8b4-4d0e-bb7b-11331ec79f6a', N'Clean_up_before_you_slip_up.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (32, N'78720d12-95b5-47e9-af74-81d5db04bd93', N'Climbing_and_Reaching_Safely.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (33, N'c3d30106-dd21-4272-a8de-1e8547dd4252', N'Communication_Tips_for_a_Diverse_Workforce.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (34, N'674a6e7a-2e49-43fc-ac9f-e16b7d85ad52', N'Confined_Space_Atmospheric_Controls.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (35, N'38da7420-aacd-4cd3-8fae-b0852418953a', N'Confined_space_entries_are_never_routine.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (36, N'55abb1b3-43e4-41de-a7f2-d104bc708110', N'Confined_Space_Hazards--Engulfment.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (37, N'de1c4848-d261-459f-a014-d48f41a6deba', N'Confined_Space_Hazards--Explosive_Combustible_Atmosphere.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (38, N'25aa2ec7-783f-443a-8f83-27a484bd7f2b', N'Confined_Space_Hazards--Oxygen_Deficiency_or_Enrichment.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (39, N'1142d9b1-2b21-49d8-bfbf-8ba8a65d44d2', N'Confined_Space_Hazards--Physical_Conditions.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (40, N'56297cc8-4dd9-4fa7-8109-42d0d78e6bdf', N'Confined_Space_Hazards--Toxic_Contaminants.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (41, N'f8e997b1-09c8-4fcd-abd5-fb847cb5f652', N'Confined_Space_Pers_Roles_and_Resp--Attendant.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (42, N'57fd6c15-8bc6-46d8-8acd-807d2a65ae03', N'Confined_Space_Pers_Roles_and_Resp--Entrant.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (43, N'add574a2-92a6-434a-ba77-d4985f61663f', N'Confined_Space_Pers_Roles_and_Resp--Rescue_Team.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (44, N'52df3aa8-d3dd-45fa-b994-4a1e9bb4150a', N'Confined_Space_Pers_Roles_and_Resp--Supervisor.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (45, N'ed388914-4cd8-4756-8040-cbcf1ccd45aa', N'Correct_Ways_to_Carry_and_Move_Objects.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (46, N'76fff0a5-6aa3-4854-85fe-2ee2ea544c23', N'Cumulative_Trauma_Disorders.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (47, N'f8429ac4-4e0e-45d5-9c07-81fe19467762', N'CYB_Safety_Video_List--6-7-2016.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (48, N'18a72738-8ea3-4207-9b27-90ce5b0e728a', N'Dont_Let_Your_Workplace_Be_a_Target_for_Robbery.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (49, N'9a383e28-0ffc-4afe-8bee-8f73691e50dc', N'Dot_Labels_and_Placards.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (50, N'cc71e713-7178-4afb-bdc3-f34fc8ce2dae', N'Do_push_it--Dont_pull_it.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (51, N'5e01ff91-42ed-4baf-b363-db8e0356c7e1', N'Do_your_part_to_prevent_slips_and_falls.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (52, N'fe8fa124-cd0d-4041-8f67-4f4ceecefffb', N'Do_you_know_where_the_fire_extinguisher_is.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (53, N'ded112ff-3f02-4349-b4bf-3c4e285f8af1', N'Do_You_Know_Where_the_Fire_Extinguisher_Is_(poster).pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (54, N'5114b083-9d7f-4783-a8c3-850ac4ddce81', N'Driving_in_Bad_Weather.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (55, N'9d2234a5-c5cc-4ead-bf61-b73f8c16c466', N'EColi_What_It_Is_and_How_to_Prevent_It.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (56, N'10cec4c8-5a45-4931-bbe9-2489651afb6d', N'Eight_Steps_to_Lock_Out_Hazardous_Energy.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (57, N'bccfe170-29b0-4266-8036-aa267130b927', N'Emergencies_Involving_Corrosives.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (58, N'cd5efcf3-ebcd-4b1c-8f0f-8ea1f6e854ba', N'Emergencies_Involving_Flammables.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (59, N'dec5a88f-e8cb-4414-abc0-47a9fd6c91ba', N'Emergencies_Involving_Reactives.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (60, N'3fc9e826-1a74-4cd3-a41c-7cdeb409e2d6', N'Emergencies_Involving_Solvents.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (61, N'c4ec351f-3fac-4947-b43a-c797d1d3a268', N'Expect_the_unexpected.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (62, N'c2e8551c-623f-4c30-8e0a-a8fa2341dba1', N'Eye_Hazards.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (63, N'2cd81075-def9-4036-a0a1-4d594dbc8374', N'Facts_About_Fire_Extinguishers.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (64, N'bfa2133a-929a-42f8-9f3d-8e7dca69f869', N'Falls_hurt--Watch_where_youre_walking.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (65, N'5079cba9-ac71-4947-92c1-258be7256d92', N'Fire_Danger_of_Flammable_and_Combustible_Liquids.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (66, N'6acc102c-83e2-4c40-a807-964978a74589', N'Fire_Emergencies--What_to_Do.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (67, N'513df09a-e824-4048-9d93-b7c99eed548d', N'Fire_Extinguishers--Choosing_the_Right_One.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (68, N'22556779-716f-4d46-9f72-65b32d60f949', N'Fire_Safety_Quiz.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (69, N'5772fc44-f5e0-407a-b19e-a438e6ed9336', N'First_Aid_for_Adults_Who_Stop_Breathing.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (70, N'668d745f-3e45-4be2-b1bf-8c96c17e9375', N'First_Aid_for_a_Choking_Adult.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (71, N'2c66042e-c1b8-4743-af91-32a3343fb3e7', N'First_Aid_for_Cuts_and_Wounds.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (72, N'bda03879-ef41-4f40-ac02-5f06b409cbb5', N'First_Aid_for_Eye_Injuries.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (73, N'a33e9fb5-76bb-498c-85ba-244f20e179c5', N'First_Aid_for_Heart_Attack.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (74, N'd55e0928-abb6-4dd5-9d79-8447ba9da28d', N'First_Aid_for_Heatstroke.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (75, N'51c291d4-e050-487e-a781-99acb427dcf1', N'First_Aid_for_Minor_Burns.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (76, N'bf68da1e-fcc7-4cda-99a9-75919d882c11', N'First_Aid_for_Minor_Wounds.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (77, N'bce43b1f-b7ed-4b7c-b755-d6d490e83cfc', N'First_Aid_for_Punctures.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (78, N'b585dc12-cae4-4c64-89d7-be1e99399a32', N'First_Aid_for_Shock.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (79, N'ce53dfa1-5247-4ede-bb2d-97ae40c3cdca', N'First_Aid_for_Strains_and_Sprains.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (80, N'fd296886-fc06-4496-a9c4-cf0fe1b932f4', N'Focus_on_Leadership_Skills.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (81, N'e6f72052-a2e9-4586-b7e3-3a4efead502b', N'Food_Service_Lifting_Basics.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (82, N'c88ad415-2e69-4616-91e9-d1741c398d06', N'Food_Storage_Basics.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (83, N'4b3def94-6609-4eac-ad42-46dae9dfec83', N'Get_help_with_heavy_loads.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (84, N'754169fd-9a2e-4632-b618-85f3e3efef0d', N'Guard_Against_Machine_Injuries.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (85, N'c183f4e5-dd64-46e3-91b5-2443b54fba5f', N'Hand_Hazards.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (86, N'a021c420-faf2-452f-a2c8-2a98e0f9a503', N'Hand_Washing_101.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (87, N'dd4d5bfd-aa48-458b-b690-9b97de7820da', N'Hazardous_Chemical_Categories--Compressed_Gases.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (88, N'919602b9-93a1-4838-b8ca-b4dc02856db7', N'Hazardous_Chemical_Categories--Corrosive_Materials.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (89, N'b484ccfd-8392-40eb-a663-666d1705a4ee', N'Hazardous_Chemical_Categories--Explosives.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (90, N'a2562749-1b7b-426d-83a0-edc6e4934be5', N'Hazardous_Chemical_Categories--Flammable_Liquids.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (91, N'1f3f4674-b8ee-4626-8b73-5dc8a393d738', N'Hazardous_Chemical_Categories--Flammable_Solids.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (92, N'8ed4cf82-7fd3-42ca-b0d0-72b4d69f7e5c', N'Hazardous_Chemical_Categories--Misc_Hazardous_Materials.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (93, N'71b783ff-cf10-43d3-a749-c498c3c052e6', N'Hazardous_Chemical_Categories--Oxidizers.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (94, N'adb742c4-2154-4880-8788-a62d0de64f66', N'Hazardous_Chemical_Categories--Poisonous_Materials.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (95, N'd8ef4ed6-6bb7-4602-ab10-7f46a3749037', N'Hazardous_Chemical_Categories--Radioactive_Materials.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (96, N'ec7c909e-de8c-4cad-9f5e-fd2b3a126c9c', N'Hazardous_Material_Identification_System_Labels.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (97, N'15a9a864-b6f0-41c6-bd4a-ca1406a4bb27', N'Hearing_Hazards.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (98, N'c098c85c-1d53-4486-bdc8-d07e98b206b6', N'How_Noise_Affects_Hearing.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (99, N'5004ba37-10ef-4490-af7f-66ba8b919196', N'How_to_Use_a_Fire_Extinguisher.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (100, N'111d5f22-507b-4e04-ba87-9d1c83e385f8', N'Importance_of_Personal_Cleanliness.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (101, N'40107665-98e5-4ab5-973f-151aa12f3771', N'Keeping_Customers_Happy.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (102, N'30f57b10-0b54-4a05-9789-ac6fc67342a7', N'Keeping_Your_Cool_in_Stressful_Situations.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (103, N'54390696-9d51-48fe-acc4-039bcc00ed6f', N'Keep_Hot_Food_at_140F--Keep_Cold_Food_at_40F_(poster).pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (104, N'480d91eb-3dae-4e1b-bbd8-852dccdcc64d', N'Keep_your_distance.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (105, N'5764f82a-411f-4d0e-8544-b48aa0beaafb', N'Kitchen_Sanitation.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (106, N'0c995b10-34e7-4e72-a310-17364fd8629e', N'Knife_Handling_and_Safety.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (107, N'71c2a4f7-2709-46b1-a389-cef83b5fa050', N'Knowing_CPR_Means_You_Can_Save_Lives.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (108, N'cca1f72a-08a5-448e-a4b2-def348e33aa9', N'Know_About_Hazardous_Chemicals.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (109, N'95064e8a-ae02-40bf-bcf6-b6e9e09d928e', N'Know_Your_Restaurant_Equipment.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (110, N'a3a34174-0f04-420a-8220-5a88c566150c', N'Lead_Hazards.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (111, N'bd7788fb-4ef9-4d86-a1b3-b52d35e607c5', N'Lifting_Awkward_Loads.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (112, N'19123c64-6ae6-4241-bfd2-7cab0104e24b', N'Lifting_Basics_1.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (113, N'eac7484e-11e8-41d3-ab0d-4120d0bb4b34', N'Lifting_Basics_2.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (114, N'da97d88e-2f8c-4e76-926d-542019914589', N'Lift_with_your_legs--not_your_back.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (115, N'4aca3951-2fac-4333-9196-11fdb05e7ce1', N'Lift_With_Your_Legs__Not_Your_Back_(poster).pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (116, N'064d6640-57cb-4a3e-9d04-6b12ee53177c', N'Lockout_Tagout--De-Energize_for_Safety.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (117, N'4330ea1b-30f4-43f1-b8ee-fe1818a5a507', N'Managing_Work_Stress.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (118, N'7ce63b1e-7dcd-4966-b03d-3fc66b657702', N'Manufacturers_Warning_Labels.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (119, N'8a97b9a2-7c18-4d0d-bb89-7441e1fff4f1', N'Material_Handling--Dollies_and_Hand_Trucks.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (120, N'eff17507-fa10-422c-81ed-e7ea0e0d2b43', N'Material_Handling--Powered_Hand_Trucks.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (121, N'461c53f5-9bba-414a-9626-0d8ea9e28fa6', N'Material_Safety_Data_Sheets.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (122, N'daa724be-c90e-4076-a140-bde8e1244d53', N'NFPA_Diamond.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (123, N'd06de646-e6ea-4980-97d4-33e35f797041', N'On_or_off_the_job--keep_safety_in_mind.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (124, N'ee5b3d7c-f9d8-49b5-a9b1-5f7af1dee1bc', N'Ouch--Watch_out_for_pinch_points.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (125, N'b25e30a1-8c00-4538-9f3a-70fd38d9db7e', N'Pay_attention_to_chemical_warning_labels.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (126, N'7f4562d0-4fa7-4d15-96b3-7d21da922d35', N'Preventing_and_Treating_Burns.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (127, N'5b1e4270-10d9-45b3-aa45-ded981917b2a', N'Preventing_Back_Injuries.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (128, N'bab6e662-c274-4391-a26e-2fae11f0f6dc', N'Preventing_Explosions--Tips_for_Worksite_Safety.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (129, N'85062677-26a3-4726-92e2-00a01d930d56', N'Preventing_Fires.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (130, N'113739cc-a84b-48dd-91a7-3024ce29a76b', N'Preventing_Foodborne_Illnesses.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (131, N'bdbad885-a79a-4766-95cb-32802b60a304', N'Preventing_Musculoskeletal_Disorders.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (132, N'3aa85ce4-88b9-4122-bcbb-40f875887a78', N'Preventing_Slips_Trips_and_Falls--Steps_to_Keep_You_on_Your_Feet.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (133, N'2410847e-ec51-4986-8e08-2935f3f37d3a', N'Prevent_chemical_burns.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (134, N'30eb53dc-dab3-47e9-b517-b8e09e516ecb', N'Prevent_chemical_spills_leaks--Inspect_containers_regularly.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (135, N'ec731d72-4c5d-4ec3-884f-b4371e53d654', N'Properties_of_Hazardous_Materials_(1).pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (136, N'1d8c336d-716d-4226-9cce-f6342139006b', N'Properties_of_Hazardous_Materials_(2).pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (137, N'7f5ec328-30ae-40ef-b4f6-ac095a3ddd5c', N'Protecting_Against_Chemical_Hazards--Safe_Chemical_Use.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (138, N'1ce73092-f7f2-4880-9d8e-e9d5d90d10cf', N'Protect_Yourself_Against_Violence_in_the_Workplace.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (139, N'bd9bc122-cf5c-4abd-aafc-1da87eae5e05', N'Quiz_on_Bacteria.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (140, N'9eaf9e02-99af-49d4-9774-69e5a0192fb4', N'Reactivity_of_Hazardous_Materials.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (141, N'f7831431-2eae-4e71-ac9e-7c0af6d06550', N'Reading_Material_Safety_Data_Sheets.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (142, N'9084f414-4dfa-447a-9167-dc43c68e513a', N'Recognizing_Medical_Conditions.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (143, N'cbb358a4-70eb-4366-b0d7-c4349a424d9b', N'Report_Unsafe_Conditions_(poster).pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (144, N'229791cb-d6fd-41a2-8e18-40031f9597bd', N'Respect_electricity.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (145, N'09779339-65e6-4124-8474-883106877b64', N'Respiratory_Hazards.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (146, N'586d5f6f-3cbb-4f7f-a9ad-d16003ecf14b', N'Responding_to_an_Electrical_Emergency.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (147, N'9688c3c4-4ce3-46fb-acf7-3ba4696176d4', N'Responding_to_a_Chemical_Spill.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (148, N'de62a0ca-a025-4c0a-a3c0-5280deff9afb', N'Safety_and_alcohol_dont_mix.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (149, N'0bab98b2-ee8c-4390-a7df-da07bf8b396f', N'Safety_Belts_Can_Save_Your_Life.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (150, N'818a744b-f275-49c5-9f88-61226c257280', N'Safety_belts_keep_the_family_together.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (151, N'44073d45-96b3-400e-b668-ec9464371927', N'Safety_Health_Lifting_Basics.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (152, N'8f606db4-9d65-4dd0-a890-22fb385275d5', N'Safety_in_the_Dining_Room.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (153, N'768827ba-2aca-4104-94a5-cb2039de23d3', N'Safety_is_everyones_job.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (154, N'16eca650-12b1-47da-9a99-f66288000846', N'Safety_Means_No_Fooling_Around.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (155, N'82f5ec1d-a6f0-491d-bf86-e38e7ad60ff3', N'Salmonella--What_It_Is_and_How_to_Prevent_It.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (156, N'5c88e7dd-1a49-49fc-b189-4115cd5331e0', N'Signs_of_Alcohol_or_Drug_Abuse.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (157, N'658c2df0-fcd4-48fc-8863-44bcedd096e0', N'Skin_Hazards.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (158, N'1f7f3a40-a939-4db6-98a4-81a8740527bb', N'Slips_Trips_and_Falls.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (159, N'ae6688ae-937e-48e6-be08-c093e2e037e2', N'Stop_Horsing_Around.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (160, N'fe87c79a-c4f3-4750-b4af-db6e63d38f82', N'Store_flammables_properly.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (161, N'11634045-fc4b-4cd9-b1d2-ed858f37259a', N'Storing_Hazardous_Chemicals.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (162, N'010bc9a7-1984-4595-ba68-8a04b6b54757', N'Storm Safety.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (163, N'47f3b53e-e0b5-4931-95e6-935ec7efcc05', N'Stove_and_Cooking_Safety.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (164, N'40dd597b-bff1-4dc8-b91c-8def9400580d', N'Testing_and_Monitoring_the_Atmosphere_of_a_Confined_Space.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (165, N'0a2f27a8-3ee1-47b4-b04a-bd02eaa31a2f', N'The_Confined_Space_Entry_Permit.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (166, N'11381993-cdb8-434c-8f29-b676dc49c725', N'The_Fire_Triangle.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (167, N'481bfd8c-2376-4abf-bfd6-83684092c302', N'The_Golden_Rule_of_Safe_Driving.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (168, N'1ffaeb46-3aa8-4adc-a8ef-687257dcb91d', N'The_Hazards_of_Cold.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (169, N'96b1dd60-e10d-4f61-bbf3-969e64090b10', N'The_Hazards_of_Electrical_Shock.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (170, N'72ff4878-b463-43e6-af4d-1ebcde749a22', N'The_Hazards_of_Fatigue.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (171, N'd4247700-98fc-4f1d-a3f5-d7d65fad4a45', N'The_Hazards_of_Heat.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (172, N'9beb948b-5496-43bc-89e1-814dc8ee945d', N'The_Mechanics_of_Lifting--How_Your_Back_Works.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (173, N'54e0b442-776b-4375-947c-16b6d4d579d5', N'The_Price_of_Accidents.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (174, N'f557b2d0-d1ed-47c3-ba82-6f2620f7517a', N'Tips_for_Good_Hygiene.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (175, N'30c81e97-742e-416b-bf3b-87afc3dbe4c8', N'Understanding_Material_Safety_Data_Sheets_(MSDS).pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (176, N'4f8cef90-3b6c-46de-8cac-3e601232c462', N'Use_eye_protection.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (177, N'a55dbfbd-c0cb-4f52-8da3-44506a36f22a', N'Use_proper_head_protection.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (178, N'2a6c537d-16d9-43dc-8470-d4d97f3f5971', N'Use_proper_safety_shoes.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (179, N'08970af3-7f9e-492a-a2c1-beca402207b1', N'Use_proper_work_gloves.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (180, N'f0261858-f2ff-4cff-a8df-8701c99b4e7a', N'Use_respiratory_protection.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (181, N'42544b04-4dca-4c82-b133-358f3ee5cba8', N'Use_the_right_tool_for_the_job.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (182, N'7e9cfc44-10ca-47ad-bc5d-e88a72d92023', N'Wash_Your_Hands_(poster).pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (183, N'76a0dffc-db41-4b26-aa9e-cfc16c3dc6be', N'Watch_out_around_corners.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (184, N'67d690db-f07a-4233-ac78-ad621420946b', N'Watch_Out_Around_Corners_(poster).pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (185, N'24d9cfcd-f3d9-407b-b9e8-793eadb2ea75', N'Watch_Out_for_Incompatible_Chemicals.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (186, N'b367fbe6-9a46-478b-b9b6-1dc59e1e90d3', N'Watch_Out_for_Pinch_Points.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (187, N'b0808e8f-0261-4592-9931-960eae98aa72', N'Watch_Your_Step_on_Scaffolds.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (188, N'd8b9940c-659c-4451-ba63-46f5758a213b', N'Welding_Hazards.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (189, N'ba29c303-2917-4293-85d0-3db257f2f9fe', N'Whats_Your_Safety_Score_(quiz).pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (190, N'e0a51883-8522-40c4-ad03-875aa3a6ee5b', N'What_Is_a_Confined_Space.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (191, N'7202ff24-e428-465e-9864-c705f885b13f', N'What_You_Need_to_Know_About_Pest_Control.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (192, N'5ea3ca65-a860-482b-8690-58fc97321aaa', N'When_Someone_Is_Choking.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (193, N'a3cfb648-f211-470d-800f-3b8e90cdf797', N'Working_Near_Overhead_Power_Lines.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (194, N'6c4162ab-2ddf-45cb-93f8-3a841aadd8d4', N'Working_Safely_Around_Forklifts.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (195, N'76a22934-b370-4aa4-b55a-6f503317fb11', N'Working_Safely_on_Loading_Docks.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (196, N'30f5e6da-0925-417d-8a0c-d4c7759fe9c5', N'Working_Safely_With_Electricity.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (197, N'de829ac9-3458-4dbe-b8ff-fbf76ecca968', N'Working_Safely_With_Grinders.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (198, N'e6f2124d-a5ed-4039-8640-9e5210992072', N'Working_Safely_With_Ladders.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (199, N'7fd4fef0-84b3-4a83-9a5d-c068d99a006f', N'Workplace_Food_Safety_Inspections.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (200, N'5775b0ab-c7f8-4c02-bc18-712a9d2e234f', N'Workplace_Safety_Checklist.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (201, N'7d67fb3a-9a2a-4f8f-b8d1-ec46a5d8557d', N'Worksite_Fire_Emergencies.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (202, N'620980f9-8d15-4b23-9b53-188b4f8cdaec', N'You_cant_follow_safety_rules_in_your_sleep.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (203, N'21da5a37-b58f-474d-8678-34c1849dcc20', N'You_Can_Help_Prevent_Employee_Theft.pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (204, N'ad7fe842-14d8-4c75-83e7-a9d118aafce6', N'You_Can_Prevent_Slips_Trips_and_Falls_(poster).pdf', N'English', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (205, N'3e8c58f0-3b94-4b5c-af61-d744e0f5a0fb', N'101_maneras_de_aliviar_la_tension.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (206, N'c734e309-f967-49e4-92ea-88d14ad89e58', N'Actividad_y_responsabilidad_del_personal_que_labora_en_un_espacio_confinado--Ayudante.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (207, N'320d7fc2-eed7-4cd6-804b-87b532cddb59', N'Actividad_y_responsabilidad_del_personal_que_labora_en_un_espacio_confinado--Equipo_de_rescate.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (208, N'bc8d42be-e94a-4dc7-a4fe-a13e7496e17e', N'Actividad_y_responsabilidad_del_personal_que_labora_en_un_espacio_confinado--Ingreso.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (209, N'448a3f10-97d9-42a8-bafa-1da8d8c50a34', N'Actividad_y_responsabilidad_del_personal_que_labora_en_un_espacio_confinado--Supervisor.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (210, N'af3a0b45-49ee-4e0b-bd70-cb4f8e99b134', N'Almacenamiento_de_materiales_peligrosos.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (211, N'51ebc5c0-7025-4c6b-b352-70b91a917852', N'Almacene_adecuadamente_los_materiales_inflamables.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (212, N'b14b881a-6652-4a2d-a556-ca6335c6fe65', N'Al_levantar_cargas--haga_el_esfuerzo_con_sus_piernas_no_con_su_espalda.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (213, N'9188048d-3c12-433d-9e89-261ecdb280a7', N'Antes_de_trabajar_en_una_maquina--idesconectela.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (214, N'75f41b43-3174-45cf-8f68-bb333cfc671b', N'Aprendiendo_resucitacion_cardiopulmonar_se_pueden_salvar_vidas.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (215, N'5039dc17-440b-48d9-b4ed-232fd6e185fc', N'Atmosfera_explosiva_o_combustible.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (216, N'd9f9867a-6684-49c9-be2d-8d3a8d988e85', N'Autorizacion_para_entrar_a_un_espacio_confinado.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (217, N'b8660c5d-352c-4b76-8eeb-d65db0c33867', N'Ay--Tenga_cuidado_con_los_puntos_donde_pueda_pellizcarse.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (218, N'0bce826d-3009-4657-b0f4-245b52288c21', N'Ayude_a_evitar_resbalones_y_caidas.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (219, N'78729e23-f42d-457f-8745-ca7afbe32dd9', N'Caracteristicas_de_un_conductor_que_maneja_a_la_defensiva.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (220, N'384d6fba-8a98-455a-97b2-11a89005b1eb', N'Categorias_de_materiales_quimicos_peligrosos--Materiales_corrosivos.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (221, N'406eed35-bdff-40b0-a1a4-e49e54d21a18', N'Categorias_de_materiales_quimicos_peligrosos--Materiales_radioactivos.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (222, N'fbff1941-736f-496e-b36e-ef568b92b0bf', N'Categorias_de_materiales_quimicos_peligrosos--Materiales_venenosos.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (223, N'dc7afe08-db13-42fa-a779-96d56cf28380', N'Categorias_de_quimicos_peligrosos--Explosivos_y_agentes_explosivos.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (224, N'e3b6ac03-aa86-4bdd-80f1-829a338896a6', N'Categorias_de_quimicos_peligrosos--Gases_comprimidos.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (225, N'96ceff6b-3b4d-40aa-9828-b214b3bb810b', N'Categorias_de_quimicos_peligrosos--Liquidos_inflamables.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (226, N'bae64690-47fc-495d-895f-11c05bf4f530', N'Categorias_de_quimicos_peligrosos--Oxidantes.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (227, N'99f403f9-d0c1-4540-80d6-b4f6535a1bb4', N'Categorias_de_quimicos_peligrosos--Solidos_inflamables.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (228, N'62840c82-0e0b-4848-ac17-32aa7d22e3a7', N'Como_actuar_correctamente_en_emergencias_electricas.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (229, N'd837095f-275d-4e00-a64b-73cf665a862c', N'Como_afecta_el_ruido_a_los_oidos.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (230, N'e5c52bcb-81b5-4139-907d-ecb3fa24bc92', N'Como_alcanzar_de_manera_segura_objetos_de_sitios_altos.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (231, N'58464dfc-7565-4bd8-977f-9ac218c7a3c5', N'Como_califica_en_seguridad.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (232, N'1e3bda05-ef45-4db6-ae1e-c337afcf6849', N'Como_complacer_a_los_clientes.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (233, N'c22c8e26-adbb-4cea-96d3-07377dbfc57c', N'Como_conducir_en_mal_tiempo.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (234, N'0bc1bd8d-26ee-4b4f-b3e2-3de5eeb59d72', N'Como_escoger_y_utilizar_cascos_protectores.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (235, N'7213cf3c-df51-4a75-a576-5bc5b86ca7fa', N'Como_escoger_y_utilizar_guantes_de_trabajo.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (236, N'4150d474-4e13-449a-aa7b-f5e8e57713f0', N'Como_escoger_y_utilizar_proteccion_para_los_oidos.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (237, N'c06c9903-376f-4085-8ea7-7131656e0ebb', N'Como_escoger_y_utilizar_proteccion_para_los_ojos.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (238, N'871f5fed-f84e-478f-9ee9-b442d8373c6d', N'Como_escoger_y_utilizar_proteccion_respiratoria.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (239, N'd61791c6-6663-44d6-9200-2a06768ffe89', N'Como_escoger_y_utilizar_zapatos_de_trabajo.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (240, N'0f5ef245-2678-442a-8611-14475f4e6abc', N'Como_evitar_enfermedades_acarreadas_por_alimentos.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (241, N'eb9c8ad4-301b-4627-93fb-affa1228e1e1', N'Como_evitar_incendios.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (242, N'28c0b784-60e8-4b9e-af7e-8bcc0e41b2ce', N'Como_evitar_lesiones_de_espalda.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (243, N'a563ee3c-1bc5-4948-93ad-3d1de4be8aed', N'Como_evitar_resbalones_tropiezos_y_caidas.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (244, N'f093156a-878f-46b1-a1d4-7d44b8773a24', N'Como_evitar_y_como_tratar_quemaduras.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (245, N'6217742d-8ebc-4ec1-9d17-8764125bc083', N'Como_interpretar_las_hojas_de_datos_de_seguridad_de_los_materiales.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (246, N'31d7b10d-b0fd-49f8-b0b0-16de9e03466d', N'Como_leer_las_hojas_de_datos_de_seguridad_de_materiales.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (247, N'7a7effd5-bc4e-4d87-b4d8-2fb2e26019ff', N'Como_levantar_cargas_dificiles.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (248, N'3f382dec-520f-4376-aa24-bdd4ecf414da', N'Como_manejar_la_tension_en_el_trabajo.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (249, N'38836714-e005-49ff-96cd-ef5cb095d124', N'Como_mantener_la_calma_en_situaciones_dificiles.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (250, N'f270527f-5c5d-443d-9d40-6f48c82e511f', N'Como_prevenir_explosiones--Consejos_para_observar_medidas_de_seguridad_en_el_trabajo.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (251, N'dd7d6670-3c1f-49a8-b163-684e44fbffe9', N'Como_prevenir_trastornos_musculoesqueletales.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (252, N'99dc0d28-34de-4772-87e9-0339429e48b5', N'Como_protegerse_de_peligros_quimicos--su_guia_para_el_uso_seguro_de_sustancias_quimicas.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (253, N'f0339d83-5f82-41ff-98f2-080acc9ec160', N'Como_reconocer_condiciones_que_exigen_asistencia_medica.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (254, N'16f1bf62-8947-4419-b25f-e0bcb41a144b', N'Como_responder_a_un_derrame_quimico.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (255, N'b8db4a5d-e6ab-4c83-981d-b2e7c2d395ad', N'Como_trabajar_cerca_de_cables_de_alta_tension.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (256, N'46538f3b-7df4-422e-aa5b-6b5006636ad3', N'Como_trabajar_de_manera_segura_con_montacargas.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (257, N'928833f7-0b28-4197-8012-b213f171d41f', N'Como_trabajar_de_manera_segura_en_los_muelles_de_carga.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (258, N'34a1e406-e596-4a1e-8911-6e7f3688ad88', N'Como_trabajar_sin_riesgos_con_escaleras_de_mano.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (259, N'0d28b00a-8b3f-44d8-9b1b-e2e3fef42675', N'Como_trabajar_sin_riesgos_con_la_electricidad.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (260, N'38d08068-db81-45f9-b17f-c93acafb4c96', N'Como_trabajar_sin_riesgos_con_molinos.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (261, N'e85179d6-8474-4239-afb8-22313c11ed1c', N'Como_usar_un_extinguidor_de_incendios.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (262, N'aeca0cd4-f24a-4dfb-a89d-f8a6a76f8862', N'Concentracion_de_alcohol_en_la_sangre_y_en_su_cuerpo.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (263, N'72db93e5-9938-4d71-b264-accfe7f7deb6', N'Condiciones_fisicas.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (264, N'22d7a640-6710-4ff2-8e15-7623aa3817af', N'Condiciones_ocasionadas_por_acumulacion_de_trauma.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (265, N'030495d0-8aeb-44c3-b626-1a0de9f8aeac', N'Conozca_como_utilizar_sustancias_quimicas_peligrosas.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (266, N'e42aff18-feb7-4096-a3ee-fb1e11d6095e', N'Conozca_el_equipo_de_su_restaurante.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (267, N'edf68f41-ee7f-4690-aadd-267ae0c149c9', N'Consejos_para_buena_comunicacion_con_una_fuerza_de_trabajo.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (268, N'12d7284a-506a-40b9-8b53-10d6a209c907', N'Contaminantes_toxicos.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (269, N'9f04c25c-829c-4170-ac60-c0880b5320e4', N'Controles_atmosfericos_del_espacio_confinado.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (270, N'79f3214f-f124-4094-b695-b3064ea4c854', N'Cuando_alguien_se_esta_ahogando.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (271, N'1a9a9b32-82b8-4d03-8337-fcdea9d005b0', N'CUIDADO.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (272, N'060086bb-6ba9-425b-8e59-a8ee232fa46e', N'Cuidado_en_las_esquinas.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (273, N'dcc6ac2e-6291-4d0c-826a-8f5251f19351', N'Cuidado_especial_al_manejar_materiales_quimicos_incompatibles.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (274, N'db832808-fb94-4bc8-a4e0-3c01576cd3c8', N'Cuidese_de_las_heridas_ocasionadas_por_las_maquinas.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (275, N'aa0201ae-f17f-41f4-962b-5311d70aa639', N'Cuidese_de_los_puntos_donde_pueda_pellizcarse.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (276, N'7f6cc0e8-68bf-43d3-92d1-b96017cd25db', N'Cuide_sus_pasos_en_los_andamios.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (277, N'dd16b2c6-9918-4b79-8b4c-a29df259ed31', N'Dentro_del_trabajo--no_se_olvide_de_su_seguridad.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (278, N'707acad1-ca93-42c7-b864-3011bc4a9c19', N'Desconexion_con_seguro_y_colocacion_de_etiquetas.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (279, N'bb54c0d6-659f-466c-9eb1-f0748f992257', N'Diamante_NFPA.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (280, N'ffdda860-4cbb-47c5-b8c9-41a1942a30f9', N'Ecoli_Que_es_la_bacteria_Ecoli_y_como_debe_evitarse.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (281, N'2fef1e3c-0a91-4e3d-8035-7920bded7ba4', N'Ejercicios_para_la_espalda_1.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (282, N'8da6c933-a1c2-4e7c-a990-07b1df793da1', N'Ejercicios_para_la_espalda_2.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (283, N'cc1cdcd0-61a1-45da-88a1-3c697e821515', N'El_alcohol_y_otras_drogas_afectan_la_seguridad.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (284, N'2477a2cd-b1fd-4272-a070-61dd262cdc13', N'El_ingreso_a_los_espacios_confinados_nunca_es_cosa_de_rutina.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (285, N'76e51220-43ff-44ae-87d0-53dfbbbf3f45', N'El_precio_de_los_accidentes.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (286, N'8b22f1e0-55a8-4c90-b874-a09a9455c65c', N'El_triangulo_del_fuego.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (287, N'3920ccdc-9acb-438f-9089-0d3610bbc22d', N'Emergencias_con_materiales_corrosivos.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (288, N'b99c969c-17d3-4877-a3b1-8584cbe3787c', N'Emergencias_con_materiales_inflamables.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (289, N'44d63867-48e3-44ee-a6bb-62adb27fea3b', N'Emergencias_con_materiales_reactivos.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (290, N'aef0d001-f669-468b-b3b4-d13834476314', N'Emergencias_con_solventes.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (291, N'546cc368-0fb6-4e09-b15f-7764ff3c12f0', N'Emergencias_ocasionadas_por_incendios.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (292, N'bf6ef05e-3acf-4438-b2d0-5979a9f2c0f0', N'Emergencias_provocadas_por_incendio_en_el_trabajo.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (293, N'ae9441f9-7198-4ef3-8f00-81241511c799', N'Empuje--No_hale.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (294, N'8b290f48-cab0-4759-a33c-6ede206c4902', N'En_el_trabajo_no_hay_lugar_para_el_alcohol_ni_para_las_drogas.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (295, N'f0fe8c1b-8700-418a-9ed0-86a2adf1b90d', N'Espere_lo_inesperado.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (296, N'9f526c18-e1b6-4458-9f89-4834fcd76058', N'Estiramientos_para_la_espalda.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (297, N'dbff1a14-5d3e-4a58-b75a-4b8678758570', N'Etiquetas_de_advertencia_de_los_fabricantes.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (298, N'badd722f-1939-4253-9ef4-4ef1ad3510a5', N'Etiquetas_y_placas_requeridas_por_el_Departamento_de_Transporte.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (299, N'5153c773-a2bd-4020-99d5-3b7c9a8cb2af', N'Evite_derrames_y_fugas_de_sustancias_quimicas_inspeccione_regularmente_los_recipientes.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (300, N'6c9d1095-58b4-42ba-a56b-0674f4db8cbe', N'Evite_las_quemaduras_con_sustancias_quimicas.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (301, N'ac6b8882-cfd4-44f7-b7db-a4c71aba2025', N'Extinguidores_de_incendios--Como_seleccionar_el_extinguidor_adecuado.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (302, N'e24c4330-3ae6-4ec2-bbbb-fb781482f327', N'Falta_o_exceso_de_oxigeno.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (303, N'd56fbbf4-9695-4ded-9a9c-3f7996add650', N'Haga_el_esfuerzo_con_sus_piernas_no_con_su_espalda.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (304, N'f6ea394f-35f8-4697-9304-77c417d1443e', N'Higiene_en_la_cocina.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (305, N'7ad471b1-13e0-472e-808d-c4d3b294a8a5', N'Informacion_fundamental_sobre_el_almacenamiento_de_alimentos.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (306, N'b4d751ce-04d5-487f-b531-edca3029b9cd', N'Informacion_relativa_a_los_extinguidores_de_incendio.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (307, N'5eea59ef-bf92-4c6d-9308-8cdb6cf2efff', N'Inspecciones_de_seguridad_de_alimentos_en_los_lugares_de_trabajo.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (308, N'31d76edd-9956-46dd-9b18-bd5605506ad8', N'Las_caidas_duelen--Mire_por_donde_camina.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (309, N'74ac3477-97fc-40a8-bb42-6ddf02e72ff3', N'Las_hojas_de_datos_de_seguridad_de_los_materiales.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (310, N'39130a9b-40e7-4562-8c09-9bad02732c43', N'Lavese_las_manos.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (311, N'aec6af86-e774-45e0-969e-ed1a97737577', N'La_actitud_lo_hace_todo.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (312, N'b9d30176-c27c-41de-8f71-dbdd38255cdd', N'La_importancia_del_aseo_personal.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (313, N'5a61481c-ce90-4112-b5ce-262ee5c305e0', N'La_manera_correcta_de_asear.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (314, N'ad42d992-344d-48e1-b1c4-2c5d669850b4', N'La_regla_de_oro_para_conducir_de_forma_segura.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (315, N'35c34136-8eb1-4c50-8c4d-d7a9f0850878', N'La_seguridad_en_el_comedor.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (316, N'26e8afc7-ee2d-4d44-820e-b30267398878', N'La_seguridad_es_trabajo_de_todos.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (317, N'3b4268ac-d7c5-4201-9ca1-5c37f361b423', N'La_seguridad_no_acepta_bromas_pesadas.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (318, N'e09db46c-32d2-4813-903b-34814538cc71', N'La_seguridad_y_el_alcohol_no_combinan_bien.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (319, N'708092f1-373f-4900-a8d5-5dd47188d464', N'La_seguridad_y_el_manejo_de_cuchillos.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (320, N'336e948d-8fb9-4d69-96c6-dd3297a6f8f0', N'Limpie_para_evitar_resbalones_y_caidas.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (321, N'17674723-7e81-41a0-949f-3ee0c39a8b7b', N'Lista_para_verificar_la_seguridad_en_el_sitio_de_trabajo.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (322, N'ba0f61ad-a9c1-4098-84c4-944f3d89d4f5', N'Los_accidentes_no_ocurren_solos.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (323, N'f8b8548f-2b11-4715-84cc-707501918da1', N'Los_accidentes_perjudican_a_todos.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (324, N'c104fa78-f1d2-4986-b8d8-e09e5de61d32', N'Los_Alimentos_calientes_se_deben_mantener_140F--Los_alimentos_frios.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (325, N'0c8070f7-b7d7-4f4c-83d9-efcbd5b7dcd7', N'Los_cables_averiados_pueden_causar_un_incendio.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (326, N'66270795-8813-42db-8226-3eafd194a4cc', N'Los_cinturones_de_seguridad_mantienen_a_la_familia_unida.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (327, N'241353ba-9fa7-4042-9c19-6318582dab47', N'Los_cinturones_de_seguridad_pueden_salvarle_la_vida.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (328, N'3f5c7731-1b02-430d-bcbc-47756e5399ca', N'Los_movimientos_mecanicos_al_levantar_cargas.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (329, N'a5761f28-3284-43f3-b776-306ce7178c2d', N'Los_peligros_del_calor.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (330, N'fdbf8d6a-c04a-48bd-82e6-560cf7c29f92', N'Los_peligros_del_frio.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (331, N'5688ac85-952f-4f07-9e2f-5a985e788232', N'Los_peligros_de_la_fatiga.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (332, N'b432af75-4157-435b-815e-afe16e16b2ed', N'Lo_que_se_debe_saber_sobre_el_control_de_plagas.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (333, N'272af7c3-b682-466c-b762-569202a71c7f', N'Maneje_su_tension.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (334, N'8a59caef-3823-41ae-910e-0de79707173b', N'Manejo_de_materiales--Carretillas_de_mano_motorizadas.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (335, N'f9802723-1fa5-4b1f-99ff-035057790f8f', N'Manejo_de_materiales--Plataformas_rodantes_y_carretillas_de_mano.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (336, N'e350444f-3f03-4562-a5ac-e5d2a657e2f8', N'Maneras_correctas_para_cargar_y_mover_objetos.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (337, N'25fc0de9-1cdc-4316-9dbd-a9644f597c20', N'Mantenga_su_distancia.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (338, N'a4f445a0-8cf2-4c82-b444-fb3d550fe88b', N'Materiales_peligrosos_varios.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (339, N'11350e04-e247-4ab5-9397-2854dfdf2593', N'Norma_basica_no_retire_los_protectores.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (340, N'b1e602f2-b216-4bdd-a0a2-b08587851030', N'No_haga_bromas_pesadas.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (341, N'2df437a0-0286-4f90-9f71-a7bc254b2479', N'Ocho_pasos_para_desconectar_energia_peligrosa_con_seguro.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (342, N'c093d7bd-1acf-4eb3-936c-e902f4c8fffa', N'Patogenos_acarreados_en_el_torrente_sanguineo--Precauciones_universales.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (343, N'5fde0d44-7607-44d3-8453-f96a08238e98', N'Patogenos_acarreados_en_el_torrente_sanguineo_VIH_y_hepatitis_B.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (344, N'ba036d89-792b-47a0-8523-25417d447c58', N'Peligros_al_soldar.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (345, N'ea4fd7f4-f2fd-4a32-9bd0-2e3f571d215a', N'Peligros_del_plomo.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (346, N'e1d17d5a-0889-4657-94ec-e228f98744e0', N'Peligros_de_los_choques_electricos.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (347, N'013ac457-17c3-4b97-b6b3-0e4ebd74b414', N'Peligros_para_el_oido.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (348, N'673d5f4a-d784-4d1a-9a59-78d59ebb70c5', N'Peligros_para_las_manos.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (349, N'718ff49d-aa4f-4160-a84c-2883f8d8f540', N'Peligros_para_la_piel.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (350, N'9a020970-46d7-4ec6-8861-9df85cf8b07a', N'Peligros_para_los_ojos.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (351, N'814e4d3c-25c9-41a2-bdcd-f963b5a5dd1a', N'Peligros_respiratorios.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (352, N'f7aa9e3b-268c-48d4-a0a7-ac56ed133abe', N'Peligro_de_incendio_ocasionado_por_liquidos_inflamables_y_combustibles.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (353, N'3a3a54c5-f0b8-4dbf-b156-43d2efd966fb', N'Pero_si_solo_estaban_bromeando.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (354, N'5a3e3801-2179-4ba4-a7e5-39af222136be', N'Pida_ayuda_antes_de_tratar_de_extinguir_un_incendio.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (355, N'e2b7957f-a0cb-429d-8c1d-0e0bae3dff7c', N'Preste_atencion_a_las_etiquetas_de_advertencia_sobre_sustancias_quimicas.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (356, N'9a6b2d00-4bc8-442b-a731-9c70cf9611e6', N'Primeros_auxilios_cuando_se_ahoga_un_adulto.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (357, N'c1d09a6d-e15f-47d0-b423-2847c13371ec', N'Primeros_auxilios_cuando_un_adulto_deja_de_respirar.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (358, N'b8f61472-76b1-4c55-bf3f-d32a97c6d6e7', N'Primeros_auxilios_en_casos_de_cortadas_y_heridas.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (359, N'38ae9ec6-6ad0-4a2e-b1c7-28a36a95bd37', N'Primeros_auxilios_en_casos_de_heridas_por_perforacion.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (360, N'c56c86c8-5178-4397-aa51-13bea1d7f84c', N'Primeros_auxilios_en_casos_de_insolacion.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (361, N'eec28dcf-e882-4d49-9d83-1a404741d561', N'Primeros_auxilios_en_caso_de_ataque_al_corazon.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (362, N'94d4adc6-67b0-4d87-88f7-cad99a6f8d52', N'Primeros_auxilios_en_caso_de_conmocion_ataque_o_choque.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (363, N'c316f79a-ecc4-4953-81f3-6364b9396bb2', N'Primeros_auxilios_en_caso_de_distensiones_o_luxaciones.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (364, N'36477a25-f231-4bb8-b830-16d6f012c297', N'Primeros_auxilios_en_caso_de_heridas_menores.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (365, N'ebb50fc5-9244-43f1-93e5-d04cb9500804', N'Primeros_auxilios_en_caso_de_lesiones_en_los_ojos.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (366, N'60c344ca-e158-4371-8d0d-44e988a1dd3f', N'Primeros_auxilios_en_caso_de_quemaduras_menores.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (367, N'fd77cacb-7f7f-4be9-a58c-e18e0f0e1bda', N'Principios_basicos_sobre_como_lavarse_las_manos.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (368, N'492c6571-a6b6-46b3-be71-17a453749177', N'Propiedades_de_los_materiales_peligrosos_(1).pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (369, N'b31caddf-5533-48f5-b2be-f16408d2329e', N'Propiedades_de_los_materiales_peligrosos_(2).pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (370, N'5d3cd660-dd11-42b2-ae96-a8315aedf588', N'Protejase_contra_la_violencia_en_el_lugar_de_trabajo.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (371, N'1ea60f9e-eaa4-4fd7-b7d0-4a27d348c172', N'Protejase_los_ojos.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (372, N'70999795-5d01-4201-b1d2-aee8bf38fd63', N'Proteja_su_lugar_de_trabajo_contra_robos.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (373, N'bbea661b-b483-4dab-abd9-9285b5222f30', N'Prueba_sobre_bacterias.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (374, N'd95bab74-4e22-4e74-84c9-6f4a0646e4d2', N'Prueba_sobre_seguridad_en_caso_de_incendio.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (375, N'2df37f02-5c93-499e-b0ed-4193919c3e64', N'Que_es_un_espacio_confinado.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (376, N'504585d1-d428-4298-ad72-9a6d79f18000', N'Reactividad_a_materiales_peligrosos.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (377, N'5cd1b682-348c-4a95-8469-36462fba5675', N'Refuerce_sus_aptitudes_de_liderazgo.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (378, N'6b2b91e7-0f83-46be-894e-9315b0347725', N'Reglas_basicas_para_la_operacion_de_montacargas.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (379, N'8987bdb0-fd5c-4ee0-b136-f81151a0041b', N'Reglas_basicas_para_la_utilizacion_de_herramientas_electricas_portatiles.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (380, N'd52ca899-f5ea-4a40-b120-0e13593f11f7', N'Reglas_basicas_para_la_utilizacion_de_herramientas_manuales.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (381, N'fc2b2667-28c7-4c62-9636-e157983fa2c0', N'Reporte_las_situaciones_que_comprometan_la_seguridad.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (382, N'9f05ceb7-7337-46be-b3bd-a8bc073fe6ee', N'Resbalones_tropiezos_y_caidas.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (383, N'9b30aa5b-ff89-4a9f-9e33-a479652ed14f', N'Respete_la_electricidad.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (384, N'0eabcbcd-8086-4d8f-bcac-7d28066586f3', N'Sabe_donde_esta_el_extinguidor_1.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (385, N'1c64fdd2-697c-4c2d-a3c2-c7a0a0e00b8f', N'Sabe_donde_esta_el_extinguidor_2.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (386, N'0e93ec3e-3b94-40e1-9636-37c02482ecd1', N'Salmonella_Que_es_la_salmonella_y_como_debe_evitarse.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (387, N'4fe1e9da-4930-48a6-a6dd-a59c44215184', N'Seguridad_al_cocinar_y_al_utilizar_estufas.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (388, N'f172434e-701c-40e2-a51f-56800f456362', N'Senales_de_abuso_de_alcohol_y_drogas.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (389, N'97c2f62a-6ddc-4a90-99a6-c30257aaa4ac', N'Sistema_de_etiquetas_de_identificacion_de_materiales_peligrosos.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (390, N'774db8a6-2411-45a1-b285-9454abaec7a8', N'Si_esta_dormido_no_puede_seguir_las_medidas_de_seguridad.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (391, N'b6e95c8e-e114-4f72-823f-ddc019e036f7', N'Sofocacion.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (392, N'c12ccc59-a575-407b-9217-e52d45296f49', N'Solicite_ayuda_para_levantar_cargas_pesadas.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (393, N'f26140e9-e91d-4746-a4ca-bf54b7134661', N'Sugerencias_para_fomentar_buena_higiene.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (394, N'c23af36a-97be-4ee8-b038-752b884ff53e', N'Tecnicas_basicas_para_levantar_cargas_1.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (395, N'c6898509-8406-473f-8fe4-7911600dab02', N'Tecnicas_basicas_para_levantar_cargas_2.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (396, N'8abe6f0f-ad90-45e2-9e95-340dffb2382e', N'Una_guia_para_trabajar_con_seguridad_cerca_del_fuego.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (397, N'4d8d52bf-8686-4f18-9d7a-656787eb8546', N'Usted_puede_evitar_los_robos_cometidos_por_los_empleados.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (398, N'0872cc15-2380-44a5-9538-89ba358c22e2', N'Usted_puede_evitar_resbalones_tropiezos_y_caidas.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (399, N'af4a52a4-0177-4fc0-b006-e0defd930581', N'Utilice_el_casco_adecuado.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (400, N'd8e56d30-b95e-4422-a048-633fba462350', N'Utilice_la_herramienta_correcta_para_el_trabajo.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (401, N'5608c413-f71c-4a22-8e67-447a64b9b3da', N'Utilice_los_guantes_de_trabajo_adecuados.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (402, N'c38d444b-4643-43a6-8380-0000385cf0e8', N'Utilice_los_zapatos_de_seguridad_adecuados.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (403, N'8209c803-71e6-449a-bd24-014834e688d2', N'Utilice_proteccion_respiratoria.pdf', N'Spanish', 1)
GO
INSERT [LossControlFiles] ([ID], [Guid], [FileName], [Language], [IsDownloadable]) VALUES (404, N'30224d2e-012b-4034-b2c6-486dace7ece8', N'Verificacion_y_control_de_la_atmosfera_en_un_espacio_confinado.pdf', N'Spanish', 1)
GO
SET IDENTITY_INSERT [LossControlFiles] OFF
GO
ALTER TABLE [LossControlFiles] ADD  CONSTRAINT [DF_LossControlFiles_IsDownloadable]  DEFAULT ((1)) FOR [IsDownloadable]
GO
