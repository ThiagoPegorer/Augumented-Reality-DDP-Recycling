# Section 2.5 — EVIDENCE DOSSIER
### Every AR source read from scratch on 2026-08-17 by seven independent reader agents

**Why this document exists.** The first four attempts at section 2.5 were written by a reader who had
already formed a view. This dossier was produced instead by seven agents that had no knowledge of the
project, of the thesis argument, or of the earlier drafts. Each was instructed to report only what is
printed on the page, to give a PDF page number for every claim, to propose no structure, and to use no
evaluative adjectives of its own. Any place where a paper contradicts itself was to be quoted on both
sides.

**All page numbers in this document are PDF page numbers, 1-indexed, not journal page numbers.** They
must be converted before they enter the thesis wherever the PDF pagination differs from the published
article.

**Twenty-one files were read.** Eighteen are the augmented reality corpus. Three more (`AR Gestures.pdf`,
`AR Gestures_tekinik.pdf`, `Augmented reality-enabled knowledge management.pdf`) were included because
they sit in the LITERATURE folder and had never been used in any draft of 2.5. Part 5 identifies them.

---

# PART 1 — Malta, Webel, Fernandez-Moyano, Windhausen, Li
*(condensed from two reader agents; every page number is a PDF page)*

## Malta, A., Farinha, T., & Mendes, M. (2023). Augmented Reality in Maintenance: History and Perspectives. *Journal of Imaging, 9*(7), 142. DOI 10.3390/jimaging9070142

**It is a systematic literature review, not a concept paper.** "This is a review article" (p. 1); "we have conducted a systematic literature review (SLR) on augmented reality" (p. 2). Google Scholar plus Web of Science, IEEE Xplore, ScienceDirect; articles after 2019, back to 2017 for electric power systems (pp. 2-3). Final article count never stated. Header label says "Article" while the text says review.

**Third-party numbers it reports (these are other people's results, not Malta's):**
- p. 7: task completion 42 s (AR), 55.2 s (head-up display), **34.5 s (LCD)**. Task location 4.9 s (AR), 11.1 s (HUD), 9.2 s (LCD). Eighteen tasks.
- p. 8: execution 21.31 s faster with AR (46.8 %); orientation error 0.08 rad (AR) vs 0.36 rad (LCD); accuracy 95.3 % vs 61.7 %; Tang et al. 75 participants.
- p. 8: Aschauer, **4 min 55 s with paper instructions vs 4 min 57 s with AR remote support**.
- p. 9: 53 % of PC tasks incorrect with paper vs 13 % with remote AR.
- p. 11: disassembly 60 min average, 25 min for an experienced technician.
- p. 12: tool detector average precision 84.7 %; time and errors reduced 33.2 % and 32.4 %.
- pp. 12-15: YOLO family detection metrics, mAP 62.81 % to 99.00 %.
- p. 16: "four articles from 2019, six from 2020, three from 2021, and two from 2022".
- No statistical test appears anywhere in the paper.

**Stated limitations (p. 15):** tracking "often computer intensive and not 100% accurate"; HMD weight; gestures and sounds "easily mistaken in industrial environments"; object location and identification need "more efficient approaches"; computation cost. p. 16: cost-effectiveness and fragmentation; call for "environmental and personal studies".

**Headline sentences:** "This allows maintenance professionals to carry out interventions more efficiently and in a shorter time than would be necessary without the support of this technology" (p. 1). "As the figure shows, the most remarkable developments took place before 2016" (p. 5). "Augmented reality has become one of the most promising technologies in maintenance thanks to the ease with which it can connect the physical environment to virtual content" (p. 16).

**Internal contradiction:** the abstract's efficiency claim (p. 1) sits against its own reported cases where LCD beat AR (p. 7) and where paper and AR were equal (pp. 8-9).

---

## Webel, S., Bockholt, U., Engelke, T., Gavish, N., Olbrich, M., & Preusche, C. (2013). An augmented reality training platform for assembly and maintenance skills. *Robotics and Autonomous Systems, 61*, 398-403. doi:10.1016/j.robot.2012.09.013

**System paper plus a skills-transfer study.** Task: assembling an electro-mechanical actuator, "composed of 25 steps grouped into six sub-tasks" (p. 4). Platform: Adaptive Visual Aids, mobile AR component, vibrotactile bracelet with six vibration actuators (p. 3), tele-consultation and authoring tool.

**Participants (p. 4):** "Twenty technicians from Sidel served as participants. They had at least 2 years of experience on field assembly/disassembly operations". Ten per group, distributed using a demographic questionnaire, prior assembly capability pre-tested. Group 1 control watched an instructional video; Group 2 used the AR platform. Trained in the morning, performed unaided in the afternoon, photo book counted as an "aid".

**Results:**
- p. 4: "the number of unsolved errors was significantly smaller in comparison to the control group: (t(18) = 2.52, p = 0.02) for the AR group"; performance time and solved errors "not significantly different" with no statistic printed; traditional training averaged 1.3 unsolved errors.
- p. 5, Table 1, control vs AR: age 34.9 / 33.5; experience 4.4 / 4.1; self-rated skill 3.8 / 3.9; capability test 304.3 s / 297.3 s; **training time 682.0 s / 851.9 s**; aids used 0 / 0; performance time 517.1 s / 494.0 s; unsolved errors 1.30 / 0.30; solved errors 0.30 / 0.30.

**The dependence argument (p. 4), verbatim:** "A potential danger of AR applications is that users become dependent on AR features such as visual instructions. As a result, the user might not be able to perform the task when those features are not available or when the technology fails. Therefore AR training programs should include phases in which the amount of AR features is reduced ... and the level of provided information is reduced (e.g. only spatial hints without detailed instructions). Therefore, the level of guidance in the AR training system has to be adaptable to the current training phase. AR-based training applications must clearly differ from AR-based guiding applications, as they must actually train the user and not simply guide the user through each task. This can be achieved only by accounting for cognitive aspects in the training."

**Guidance level as a dial:** Adaptive Visual Aids operate "at a strong guidance level and (b) at a softer guidance level" (p. 3, Fig. 2); the design makes it "easy to control the guidance level" (p. 5).

**Internal contradictions:**
1. Body: performance time "not significantly different" (p. 4). Conclusion: "technicians who trained with an AR-based training platform made less errors and achieved better performance times" (p. 5).
2. Text: "The duration of one AR training cycle was only slightly higher than traditional training cycles" (p. 5). Table 1: 682.0 s against 851.9 s, which is 25 % higher (p. 5).

**Future work (p. 5):** capture and interpretation of underlying skills, recognition of user intention, large-scale capture of expert skills, integration and evaluation of a remote component, other haptic-hint devices.

---

## Fernandez-Moyano, J. A., Remolar, I., & Gomez-Cambronero, A. (2025). Augmented Reality's Impact in Industry: A Scoping Review. *Applied Sciences, 15*, 2415. DOI 10.3390/app15052415

PRISMA-ScR, protocol registered (p. 3). SCOPUS, Web of Science, ACM Library; search run 7 January 2025; 2014-2024, journal papers in English; mixed reality excluded; reviews excluded (pp. 3-4).

**Funnel (p. 4):** 396 recorded, 104 duplicates removed, 140 discarded at first screening, "final set of 74 articles"; a citation search added 6 of which 3 progressed.

**Counts:**
- p. 5: peak 2022 with 15 articles; 12 in 2023; 9 in 2024; lowest 2015 with one.
- pp. 5-6, sectors: robotics 11, automotive 9, logistics 6, construction 5, education 5, manufacturing 5, health 4, aviation 2, 20 non-specific.
- pp. 6-7, software: Unity 49, Vuforia 28, ARCore 6, AR Foundation 3, ARKit 3, Mixed Reality Toolkit 5.
- p. 7, detection: markers 34, surface detection and object recognition 12, environment recognition 10, none 3, face tracking 1, unspecified 5.
- pp. 7-8, purpose: visualization 32, maintenance 15, operator assistance 10, manufacturing 10, education and training 9, assembly 7.
- p. 8, audience: **operators 50**, workers 8, teachers 6, students 6, technicians 5, customers 4.
- p. 9, AI: **63 studies use no AI**; deep learning 10, CNN 4, others 1 each.
- pp. 9-10, hardware: smartphones 39, **smartglasses 30** (HoloLens v2 8, v1 9), tablets 15, projectors 2.
- No statistical test anywhere.

**Headline sentences:** "the implementation of artificial intelligence within augmented reality applications remains limited, revealing a significant opportunity for future research" (p. 1). "No single sector dominates, as the relevance of the studies is distributed without any of them standing out notably over the rest" (p. 11). **"AR applications predominantly remain in the research domain, with few commercially viable and rigorously evaluated interventions available in real-world settings" (p. 17).**

**Stated limitations (p. 16):** no exhaustive comparative evaluation of tools; no practical tests, so development time, frame rates and hardware performance were not measured; the AI discussion "remains general".

**Internal contradictions:** 74 articles in the abstract and results (pp. 1, 4) against "After analyzing 77 studies" (p. 10). Automotive 9 studies (pp. 5-6) against "5 out of 7" (p. 11).

---

## Windhausen, A., Heller, J., Hilken, T., Mahr, D., Di Palma, R., & Quintens, L. (2024). Exploring the impact of augmented reality smart glasses on worker well-being in warehouse order picking. *Computers in Human Behavior, 155*, 108153. DOI 10.1016/j.chb.2024.108153

**Two controlled laboratory experiments, both with students, both order picking.**

**Study 1 (pp. 5-6):** two warehouse shelves, 28 storage bins of office supplies, 30 customer order bins; sort 28 items. ARSG group n = 38 used a Vuzix headset with a ring barcode scanner; printed-list group n = 27. 65 participants, aged 21 to 29, M = 23.71, recruited for course credit.
- Overall: 9.5 min (SD 4.60), 95.22 % of orders correct (SD 0.074).
- **Time: M print 12.81 vs M ARSG 7.28, t(31) = 5.18, p < .001. Error rate: .10 vs .01, t(29) = 4.80, p < .001.**

**Study 2 (p. 6):** three rows of three shelves, eight boxes each, pick six items. ARSG group n = 142 used a Microsoft HoloLens 2; printed-list group n = 82. 224 participants aged 18 to 27.
- **Well-being: M print 4.50 vs M ARSG 5.31, t(222) = 5.19, p < .001.**
- Moderated mediation, PROCESS Model 7 (p. 7): job satisfaction, effect of ARSG use β = -1.744, p = .090, interaction with tech savviness β = 0.351, p = .039. Productivity enhancement, β = -2.490, p = .037, interaction β = 0.431, p = .029. Well-being regression: job satisfaction β = 0.601, p < .001; productivity enhancement β = 0.162, p = .004.
- Conditional effects on job satisfaction: low savviness β = 0.089, p = .645; medium β = 0.399, p = .005; high β = 0.708, p = .001. Johnson-Neyman point 5.782, with 71.875 % of the sample above it.
- Reliabilities: well-being alpha .78, job satisfaction alpha .62 (AVE 0.47), productivity alpha .80, savviness alpha .84.

**Headline sentences:** "using ARSGs directly increases perceptions of well-being compared to traditional support tools" (p. 1). "However, contrary to expectations, ARSG use does not generally lead to higher job satisfaction or perceived productivity enhancements for all workers" (p. 1). **"while ARSGs are of primary benefit to those with higher tech savviness ... they may do more harm than good for those with very low tech savviness and even adversely affect perceived productivity" (p. 7).**

**A third-party result inside their Table 1 (p. 3), worth knowing:** "ARSG users initially outperformed paper-instruction peers by 43.8 % but took 23 % longer in task repetition without aids."

**Stated limitations (p. 9):** students not order pickers; simplified warehouse; job-satisfaction reliability low and conceptually close to well-being; "relatively simple visualizations" rather than complex holograms; short exposure, so fatigue and eyestrain were probably avoided.

**How the overlay content is produced:** not stated. Study 1 scanned a bin barcode and showed red and green icons; Study 2 displayed the order list and let the user move and scale it. No sensing, data model or authoring pipeline is described.

---

## Li, J., Liu, B., Duan, L., & Bao, J. (2023). An Augmented Reality-Assisted Disassembly Approach for End-of-Life Vehicle Power Batteries. *Machines, 11*, 1041. DOI 10.3390/machines11121041

**Table 1, p. 15, verbatim ("Comparison of different disassembly methods (unit: seconds)"):**

| | Round 1 | Round 2 | Round 3 | Average |
|---|---|---|---|---|
| Group A | 325 | 316 | 307 | 316 |
| Group B | 277 | 271 | 268 | 272 |
| Group C | 261 | 260 | 276 | 265 |
| Group D | 267 | 263 | 260 | 263 |

No standard deviations, no test statistics, no p values, no footnote.

**Group definitions, p. 14, verbatim:** junior = "less than one year of experience", or disassembly efficiency "within the lowest 20% of their peers", or disassembly quality "in the lowest 20%". Skilled = "more than three years" and "top 20% in terms of disassembly efficiency and disassembly quality". "Group A consisted of junior workers who were guided by a technical manual, while group B comprised junior workers who received assistance from an AR-assisted system." "Group C consisted of skilled workers without AR assistance, and group D comprised skilled workers equipped with wearable AR devices but who did not utilise the system for disassembly." Groups C and D exist "to determine if wearing AR devices would hinder their disassembly tasks".

**Design, p. 14:** four groups, ten workers each; three rounds, ten replicates per round; identical battery packs of the same model and specification; "the ultimate evaluation metric was established as the time needed to meet the disassembly quality standards". Three display modes: text-based, image-based, and a simplified version emphasising tasks and tools.

**System, pp. 5-13:** HoloLens, Unity3D, Visual Studio, client-cloud model. Mask R-CNN with ResNet101 on a Coco pre-trained model, then depth segmentation, point-cloud reconstruction and ICP registration for pose. "the disassembly steps are retrieved based on a predetermined disassembly sequence diagram, and the corresponding AR guidance is displayed" (pp. 5-6). "The AR-assisted disassembly instructions required for ongoing disassembly tasks are pre-generated ... which avoids the intervention of the human operator" (p. 7). Attitude data returned to the client as JSON (pp. 12-13). Because Mask R-CNN and ICP are too heavy for real time, "the algorithm is activated every 10 s" (p. 13).

**The tool-attention mechanism, p. 11:** showing every recognition result "can result in an information overload, hindering operators from comprehending the key points amidst the abundance of chaotic information", and "a substantial quantity of instance segmentation results could obscure real objects". So the system infers intent from the tool in the operator's hand: "when the operator grasps a cross screwdriver ... the AR glasses exclusively highlight and display all the screws that need to be unscrewed".

**Stated limits, p. 15:** the batteries were "limited to mainstream models commonly encountered within the current recycling channels"; "established operational procedures and instructions for disassembly were available". p. 12: deep learning on the wearable itself is "challenging, due to limitations in hardware and software".

**Internal contradictions, four of them:**
1. **p. 14 attributes 316 s to the AR group.** Table 1 gives 316 s to Group A, the technical manual. The 13.9 % improvement is arithmetically correct for 316 to 272, so the percentage stands and the labelling does not.
2. **p. 15: "When skilled workers utilised AR glasses for assistance, the disassembly time was reduced to 83.3% of the original duration."** No pair in Table 1 gives 83.3 %; the closest is Group D over Group A, 263/316 = 83.2 %, which compares a group that did not use the system against a group with a paper manual. Group D is defined on p. 14 as not using the system.
3. **The abstract claims significance and error reduction that were never measured.** "The results demonstrated that this approach significantly enhanced disassembly efficiency and decreased the frequency of disassembly errors" (p. 1), while p. 14 records only disassembly time and no test appears anywhere.
4. Group C's printed average, 265, is not the mean of its rounds, 265.67 (p. 15). Dataset size is 800 in Figure 3 (p. 9) and 1000 in the body (p. 10).

---

# PART 2 — Chen, Ariansyah

# Evidence profile: two PDFs

---

## Paper 1 — `Chen2023_AR_enabled_HRT_for_waste_sorting.pdf` (21 PDF pages)

### Citation as printed
- **Authors (title page, p. 1):** "Junjie Chen a, Yonglin Fu a, *, Weisheng Lu a, Yipeng Pan a"; affiliation "Department of Real Estate and Construction, The University of Hong Kong, Pokfulam Road, Hong Kong, China"; corresponding author "u3009531@connect.hku.hk"
- **Self-declared citation printed on p. 1:** "Chen, J., Fu, Y., Lu, W., & Pan Y. (2023). Augmented reality-enabled human-robot collaboration to balance construction waste sorting efficiency and occupational safety and health. *Journal of Environmental Management*, 348, 119341. DOI: 10.1016/j.jenvman.2023.119341"
- **Year:** 2023 | **Journal:** Journal of Environmental Management | **Volume:** 348 | **Issue:** not printed | **Page range:** not printed (article number 119341) | **DOI:** 10.1016/j.jenvman.2023.119341
- **Version:** pre-print, stated explicitly on p. 1: "This is the pre-print version of the paper:" and "The final version of this paper is available at: https://doi.org/10.1016/j.jenvman.2023.119341." Continuous line numbers appear in the right margin throughout (1–679); no publisher typesetting, no journal running head. PDF metadata producer: "Microsoft® Word 2019".
- **Keywords as printed (p. 1):** "Construction and demolition waste; Automated waste sorting; Human-robot collaboration (HRC); Augmented reality (AR); Occupational safety and health (OSH)"

### Type of work (paper's own words)
System-development / prototype paper with laboratory experimental demonstration.
- p. 1: "Firstly, a model for human-robot collaborative sorting using AR is established. Then, a prototype for the AR-enable collaborative sorting system is developed and evaluated."
- p. 6: "3 AR-enabled waste sorting system development — This section aims to develop a prototype AR-enabled system based on key concepts of the model proposed in Section 3."
- p. 13: "4 Experimental demonstrations"; "To show the effectiveness and feasibility of the proposed system, experiments were conducted in the laboratory environment."
- p. 16: "To validate the feasibility of the AR-enabled model, a series of experiments are conducted."; "Fig. 8b presents the results of the repeatability test for two methods in two conditions."

### What was studied
- **Task (pp. 13–14):** sorting non-inert construction waste out of mixed construction waste on a stationary workstation. "This research focuses on quality control in CWS operation, where non-inert construction materials (e.g., plastic and wood) should be sorted out before the CW stream were conveyed to the next sorting process." (p. 13) "Several types of common CW are selected, including wood, concrete, gravel, glass, and plastics." (p. 14)
- **System (pp. 6–13):** four modules — perception module (RGB-D sensor), robotic sorting module (robotic arm + vacuum gripper end-effector), AR module (Microsoft HoloLens 2, Unity), communication module (ROS). Hardware/software as printed on p. 13: "The experiments were conducted using UR5e and OnRobot VG10. RealSense D435i was used as the depth sensor installed on the end-effector for waste detection. Robot control and image processing were managed by ROS (version Noetic) on a single laptop with Ubuntu 20.04 OS. AR application was developed by Unity (version 2021.3.7f1) on Windows 11 OS. The AR head-mounted display, Microsoft HoloLens 2, was used to run the holographic medium."
- **Participants:** **no human participants are reported.** No number, background, recruitment, ethics approval, allocation, or training procedure is stated anywhere in the paper. The human role is described generically as "the worker"/"operators"/"human operator" (e.g., p. 14: "the worker and the robot arm stand on either side of the CW"). Sample count refers to waste samples, not people: "Each condition comprises twenty groups of samples with varying compositions and locations." (p. 16)
- **Conditions compared (p. 16):** two methods × two scenarios. Methods: "the AR-enabled HRC method and the conventional model-based method"; in Fig. 8b labelled "Without HRC" and "HRC". Scenarios: "sorting mutually isolated CW and sorting mixed CW with obstructions".

### Every quantitative result (with PDF page)
| Value as printed | Page |
|---|---|
| "improve the accuracy rate of CWS by 10% and 15% for sorting isolated CW and obscured CW, respectively, when compared to the method without human involvement" (abstract) | 1 |
| "CW accounts for up to 40% of the total solid waste in some cities (Hoornweg & Bhada-Tata, 2012)" — cited, not this study's result | 2 |
| "automatically sort CW with a size range of 0.05-0.5 meters" (Xiao et al., 2020) — cited | 2 |
| "enhanced the accuracy of robotic grasping from 70% to over 90%" (Ku et al., 2021) — cited | 2 |
| Eq. (1), coordinate conversion: "[P′x, P′y, P′z] = [−Py, Pz, Px]"; "[Q′x, Q′y, Q′z, Q′w] = [Qy, −Qz, −Qx, Qw]" | 13 |
| Equipment/version numbers: UR5e; OnRobot VG10; RealSense D435i; ROS Noetic; Ubuntu 20.04; Unity 2021.3.7f1; Windows 11; HoloLens 2 | 13 |
| "Each condition comprises twenty groups of samples with varying compositions and locations." | 16 |
| Eq. (2) "Accuracy rate = TP/N"; Eq. (3) "Grabbing rate = SG/TP"; with "N denotes the number of non-inert CW that need to be sorted in the scenario; TP denotes true positives, where positive samples are correctly classified. SG denotes the number of successfully grabbed correct samples." | 16 |
| "When sorting isolated CW, our HRC method increases the accuracy rate by 10% compared to the method without human involvement." | 16 |
| Fig. 8b bar charts "Isolated CW" and "Obscured CW", series "Without HRC" and "HRC", categories "Accuracy rate" and "Grabbing rate"; y-axis printed 70.00% to 100.00% in 5-point gridlines. **No data labels or numeric values are printed on the bars**, and no table of underlying numbers appears in the paper. | 16 |
| "The improvement in accuracy rate is more pronounced for sorting obscured CW, with an increase of 15%, as the traditional method encounters more challenges." | 17 |
| "However, the accuracy rates of sorting obscured CW are around 90% due to the limitations of our object segmentation method based on traditional image processing." | 17 |
| "In terms of success rate in grabbing, both traditional and HRC methods perform almost perfectly, with a success rate of over 98%, as the experiment focused solely on removing flat non-inert materials." | 17 |
| "The experiments show that the proposed HRC method improves the accuracy of sorting isolated CW and obscured CW by 10% and 15%, respectively." | 17 |
| UI element counts: "there are three interaction buttons about categories of waste on the left (i.e., wood, glass, and plastic) and two interaction buttons about the functions on the right" | 10 |

**Statistical tests:** none printed. No test statistic, p value, confidence interval, standard deviation, or variance measure appears anywhere in the paper. No completion time, cycle time, or throughput is measured or reported.

### What was measured beyond time and errors
Only two outcome indicators are defined and reported, both named on p. 16: "accuracy rate" and "grabbing rate" (also called "success rate in grabbing", p. 17). There is **no** eye-tracking, workload, usability, acceptance, questionnaire, physiological, or other subjective measure anywhere in the paper. OSH is discussed qualitatively (pp. 2, 4–6, 17) but no OSH metric is measured. Time is not measured either.

### How the system decides what to show the user
Two named information levels produced by the perception module drive both the robot and the overlay:
- p. 7: "There are two levels of information. The first level of information (FLI) consists of the location and category of the waste, meaning that the waste can be detected using the object detection algorithm. The second level of information (SLI) is obtained by morphology-based object segmentation, which only indicates the location of the potential waste without the categories being known. The FLI can be used directly for automatic waste sorting. If FLI is not available, the human operator needs to use the SLI and manual instruction to guide the sorting task."
- p. 7: "This perception task is delegated to an RGB-D sensor (RealSense D-series sensor in this study), which provides RGB images and depth information together".
- p. 8, FLI source: "the objects detection task is delegated to the find_object_2d (Labbé, 2011), an object recognition package of ROS that integrates OpenCV (Bradski, 2000) to implement SIFT, SURF, FAST, BRIEF, and other feature detectors to recognize objects from a pre-prepared database." "The first step is the demonstration, where the object is presented to the vision system, and the image and the extracted feature set are stored as patterns in the database. The dataset of waste should be prepared before the process of automated waste sorting."
- p. 8, SLI source: "the morphology-based approach is adopted, which is a sequence of traditional image processing methods… The raw image will be enhanced contrast through histogram equalization. Then the RGB image would be converted into a gray image, and be threshold processed… the morphologic erosion method and morphologic dilation method will be acted on the image successively… Finally, minimum enclosing rectangles will be calculated for each region, and SLI will be provided for further tasks."
- p. 10, overlay rendering: "The augmented information is displayed on the real waste as semi-transparent cubes with different colors, showing corresponding detected categories to operators. Through AR glasses, operators can monitor the FLI in real time. One of the detected FLI consists of a color box and a brand, which shows the detected category and the corresponding location. Besides, the virtual robot can indicate the next motion of the real robot".
- p. 10, human override path: "The first function button is 'Mark me', with the function of manual mark the waste by using the SLI."; "When finding one with wrong detection (e.g., detect the plastic waste as the wood), operators can click the second function button of 'Remark me', in which operators can select the waste with wrong detection and select the correct category based on experience, so that the robot can conduct the correct gripping."
- p. 10, safety content: "The operator is alerted to a safety warning that is projected in the operator's field of view, indicating the operator is close to the robot and there may be a safety risk to the operator. This alarm will be automatically released when the operator moves away from the robot."
- p. 11, transport of content: "FLI and SLI are published at the same time during the detection stage, and humans can select the interested information to visualize through the AR module. When one is selected, corresponding prefabs about the waste information will be created and shown in the AR environment." "The final information flow indicates the planned trajectory of the robot arm is visualized to the human, which is published from ROS to the AR module."
- p. 12 (Fig. 5b labels, image): "Publish information of waste detection (material, position, rotation)" ①; "Publish information of waste segmentation (position, rotation)" ②; "Publish the locations and materials of detected objects" ③; "Request service of pick & place with corresponding materials" ④; "Publish the planned trajectory of robot arm" ⑤.
- p. 12, registration: "The virtual robot represented in Unified Robot Description Format (URDF) in Unity has the same dimensions and configuration as the real robot, which should be aligned together by an AR marker (set manually closed to the real robot). The AR marker can be removed after the pose of the virtual robot has been initialized."

### Authors' own limitations and future work
All on p. 18 unless noted.
- "Nevertheless, the research remains several limitations. Firstly, the object segmentation in the proposed system is based on traditional image analysis, which can result in errors, such as considering two waste items as a single large object. The stability and reliability can be improved by using advanced computer vision methods in the future, such as the Segment Anything Model (Kirillov et al., 2023)…"
- "Secondly, the level of human involvement in the proposed system is limited to modifying the wrong category detected by the sensors. Although our experiments did not demonstrate the improvement of successful grabbing rate, human instruction has the potential to tackle challenges in optimizing grasping pose through AR devices. The proposed system can be further extended to provide optional grasping poses and grasping preferences…"
- "In addition, as the study only intends to provide a new HRC framework for CWS, the experiments were limited to the stationary workstation, while the CW should be placed on a moving conveyor in an actual recycling line. Practical applications in the real setting of the proposed AR-enabled CWS system are needed to validate."
- "Moreover, the economic and safety challenges of scaling up our prototype should be investigated in the future. The cost-effectiveness and feasibility of the proposed system should be evaluated, along with assessing and minimizing potential hazards associated with the interaction between humans and robots, as well as the risks of system failure or malfunction."
- p. 17 (in-line limitation): "It is worth noting that the current HRC method only enhances the recognition accuracy rate based on visual methods and does not optimize grasping actions, such as determining the optimal grasp location."
- p. 17: "Our HRC model excels in removing large obstructed waste over mixed CW instead of CW with only a small overlap."
- p. 18 (conclusion): "Future research should explore opportunities to enrich the proposed method by incorporating deep learning techniques and increasing the level of human involvement. Additionally, there is a need to expand the application of the proposed method beyond the research setting, such as through practical implementation in an industrial environment."
- p. 13 (generalisation claim adjacent to limitation): "It should be noted that although these experiments were only conducted in the case of sorting out the flat and non-inert construction waste, the system can be generalized to other CWS operations (e.g., sorting inert construction materials) after configuration, even to the general solid waste sorting operations."

### Five verbatim headline claims
1. "The experimental results demonstrate that the proposed AR-enabled HRC method can improve the accuracy rate of CWS by 10% and 15% for sorting isolated CW and obscured CW, respectively, when compared to the method without human involvement." (p. 1)
2. "The research lays out a human-robot collaborative paradigm for productive and safe CWS via an immersive and interactive interface like AR." (p. 1)
3. "In this model, humans do not need to bend down and touch the waste." (p. 6)
4. "The primary contribution of the research is developing an AR-enabled HRC method for CWS that leverages the advantages of both humans and robots." (p. 17)
5. "It is found that human vision and learned experience can be used to enhance the robotic sorting performance through the AR-enabled HRC sorting system." (p. 18)

### Theoretical model or framework invoked
No named cognitive, behavioural, or human-factors theory is invoked. The interpretive constructs are the authors' own model and the borrowed definition of HRC:
- p. 4: "2 A model for human-robot collaborative waste sorting"; p. 5: "2.2 A model for AR-enabled HRC in CWS".
- p. 4: "HRC refers to a collaborative process where humans and robots can work together to achieve shared goals in a shared workspace, to make the best use of both human and robot capabilities (Robla-Gómez et al., 2017)."
- p. 5, three named AR advantages used as the organising frame: "Visualization.", "Communication.", "Ergonomics. Both cognitive and physical ergonomics can be improved through AR technology in HRC (Khamaisi et al., 2021)."
- p. 6–7: FLI/SLI two-level information construct (authors' own).

### Internal inconsistencies
1. **Cross-reference to a non-existent section (model section number).** p. 6: "This section aims to develop a prototype AR-enabled system based on key concepts of the model proposed in Section 3." The model is Section 2 — p. 4: "2 A model for human-robot collaborative waste sorting" — and Section 3 is the section making the reference: p. 6 "3 AR-enabled waste sorting system development".
2. **Reference to a non-existent subsection 5.2.** p. 10: "they can remotely click this button, and SLI will be shown to them in an AR environment with every gray cube (see Section 5.2)." Section 5 is "5 Discussion" (p. 17) and has no subsections.
3. **Wrong figure number in the results walk-through.** p. 14: "Fig.8c illustrates the scenario in which the perception module detected the wrong category of waste, and the plastic has been wrongly detected as glass (#1 of Fig.7c)." Fig. 8 is captioned "(a) Experiment samples; (b) Experimental results" (p. 16), and the panel described is Fig. 7c — "(c) scenario that the perception module detected the wrong category of waste" (p. 16).
4. **Wrong figure number for the UI.** p. 10: "As shown in Fig.6, there are three interaction buttons about categories of waste on the left (i.e., wood, glass, and plastic) and two interaction buttons about the functions on the right". Fig. 6 is captioned "The configuration of the experiment platform" (p. 13); the category and function buttons are shown in Fig. 5a, captioned "(a) The view in AR glasses" (p. 12), whose image labels read "Category Buttons", "Function Buttons", "Mark me", "Remark me".
5. **Worker position/contact with waste described both ways.** p. 5: "Workers standing by the conveyor belt can easily recognize the category of waste and understand how to pick it up." Against p. 3: "the difficulties of CWS are mainly about waste recognition and location, which provide opportunities for humans to stand away from the conveyor belt and eliminate the hazards of manual sorting"; p. 6: "In this model, humans do not need to bend down and touch the waste. Instead, they can enhance the sorting quality by monitoring and modifying the information obtained from the perception system."; p. 5: "It also allows workers to collaborate with robots at a certain distance".
6. **Headline percentages are not traceable to any printed number.** The 10% and 15% improvements (pp. 1, 16, 17) and the "around 90%" and "over 98%" figures (p. 17) are supported only by Fig. 8b (p. 16), which prints no data labels and no underlying table; whether the improvements are percentage points or relative percentages is not stated.

---

## Paper 2 — `AR cognitive.pdf` (18 PDF pages)

### Citation as printed
- **Authors (p. 1):** "Dedy Ariansyah a,*, John Ahmet Erkoyuncu a, Iveta Eimontaite b, Teegan Johnson b, Anne-Marie Oostveen b, Sarah Fletcher b, Sarah Sharples c" — a "Centre of Digital Engineering and Manufacturing, School of Aerospace, Transport and Manufacturing, Cranfield University, Bedford MK43 0AL United Kingdom"; b "Industrial Psychology and Human Factors Group… Cranfield University"; c "Human Factors Research Group, Faculty of Engineering, University of Nottingham".
- **Title (p. 1):** "A head mounted augmented reality design practice for maintenance assembly: Toward meeting perceptual and cognitive needs of AR users"
- **Journal / volume / year / article number (running head, all pages):** "Applied Ergonomics 98 (2022) 103597"
- **Issue:** not printed | **Page range:** not printed (article number 103597) | **DOI (p. 1):** "https://doi.org/10.1016/j.apergo.2021.103597"
- **Dates (p. 1):** "Available online 28 September 2021"; "Received 24 June 2021; Received in revised form 25 August 2021; Accepted 22 September 2021"; "0003-6870/© 2021 The Authors. Published by Elsevier Ltd. This is an open access article under the CC BY license".
- **Version:** typeset published version. Elsevier two-column layout, journal running head and page numbers 1–18, ScienceDirect banner, no margin line numbers, no "accepted manuscript" wording. PDF metadata subject field: "Applied Ergonomics, 98 (2022) 103597. doi:10.1016/j.apergo.2021.103597".
- **Keywords as printed (p. 1):** "Augmented reality / Head-mounted display / Eye gaze behaviours / Joint cognitive system / Usability / Multiple resource model"

### Type of work (paper's own words)
Between-subjects controlled user experiment with questionnaire and physiological/eye-tracking instrumentation.
- p. 1: "This paper uses a theoretically-driven approach to interaction design to investigate the impact of different AR modalities in terms of information mode (i.e. video vs. 3D animation) and interaction modality (i.e. hand-gesture vs. voice command) on user performance, workload, eye gaze behaviours, and usability during a maintenance assembly task."
- p. 6: "4 Methods / 4.1 Participants — A total of sixty-three volunteers recruited through email and an announcement on the University's intranet homepage participated in a paper-based manual and AR-based maintenance task."
- p. 8: "There were two main conditions in which this experiment was carried out."
- p. 9: "4.4 Data analysis — Statistical computation was performed on the data collected to determine whether there was a statistically significant impact of independent variables."
- p. 6: "this paper attempts to test four key research questions" (listed p. 6).

### What was studied
- **Task (p. 8):** "The maintenance task was designed to represent a real-life situation in which the participant was presented with a gearbox machine that had a maintenance problem (i.e. the brake is not working, and they were asked to replace the brake)." Sub-tasks: "1) searching the maintenance instructions, 2) processing what they mean, 3) locating where it is applied, 4) working on the maintenance task (1. Unscrew the transparent cover, 2. Remove the transparent cover, 3. Unscrew the support that holds the brake wheel, 4. Remove the current brake piece, 5. Replace the brake with a new piece, 6 Tighten the support that holds the brake wheel, 7. Put back the transparent cover, 8. Tighten the transparent cover), and 5) transitioning from one to another instruction until the whole task is done." p. 9: "The maintenance task involved a disassembly and assembly process using the combination of bare hands and some Allen keys."
- **Device/system (p. 6):** "a gearbox machine, which is commonly used for demonstrating condition-based monitoring for teaching purposes at Cranfield University"; "a Microsoft Hololens 2 (Microsoft, 2019a) was used as the HMD that consists of a holographic display and user input sensors including hand-detection, a microphone, and eye-tracking"; "Vuforia version 8.3.8 object tracking (PTC, 2019) was used for the AR tracking library and registration. The virtual content management, user interface, and data collections were programmed in Unity version 2019.1.10f (Unity, 2019) and interfaced with a local web and database server." Also Empatica E4 wristband (p. 7).
- **Participants (p. 6):** "A total of sixty-three volunteers… Nine of them were excluded due to incomplete data caused by errors in the technical systems, and three of them were excluded due to age-related performance outliers (>50 years)." "The background of the volunteers was a combination of students and staff with varying levels of maintenance experience and exposure to AR technologies. Their mean age, gender, maintenance and AR experience were balanced across the groups and are presented in Table 3." "All volunteers reported being healthy and not having any musculoskeletal injuries in the past 12 months." p. 9: "the participants were recruited from the same environment and were randomly balanced". Consent/ethics, p. 7: "participants read and signed a consent form that was approved by the Cranfield University Research Ethics System."
- **Group sizes (Table 3, p. 7):** CG N = 10 (M = 7; F = 3); VH N = 11 (M = 9; F = 2); AH N = 10 (M = 9; F = 1); VV N = 10 (M = 9; F = 1); AV N = 10 (M = 9; F = 1).
- **Conditions compared (pp. 8–9):** one paper-based control group plus a 2 × 2 AR design. p. 9: "For the AR-based maintenance system, there were four groups with a combination of information mode and input modality such as video-hand gesture (VH), video-voice command (VV), animation-hand gesture (AH), and animation-voice command (AV)." Control, p. 8: "one group of participants was asked to perform the maintenance task using a paper-based manual… This group served as a control group (CG)." Visualisation always-on, p. 8: "Both visualisations were set to always-on mode rather than on-request as suggested in previous research (Kim et al., 2019)."

### Every quantitative result (with PDF page)

**Demographics and correlations — Table 3 (p. 7)**
Mean (SD) age: CG 26.6 (4.55); VH 26 (3.77); AH 25.2 (3.93); VV 27.5 (4.69); AV 26 (4.47). Maintenance experience (number/5 yrs): CG 219.5 (437.05); VH 11.45 (16); AH 11.6 (16.43); VV 32.5 (50.62); AV 28.3 (39.08). AR exposure (number/5 yrs): CG 3.6 (9.33); VH 4.72 (15.02); AH 3.9 (5.95); VV 2.2 (6.28); AV 0.7 (1.15). Spearman's coefficients — CT–Age: CG −0.48, VH 0.37, AH 0.60, VV −0.46, AV −.739*; CT–Mexp.: CG 0.01, VH −0.07, AH −0.01, VV −0.36, AV -.779**; CT–ARexp: CG −0.18, VH −0.42, AH −0.08, VV 0.07, AV 0.35; Error–Age: CG −0.22, VH .602*, AH −0.36, VV −0.36, AV "–"; Error–Mexp.: CG 0.09, VH .725*, AH −0.41, VV −0.11, AV "–"; Error–ARexp: CG 0.49, VH −0.41, AH −0.06, VV 0.49, AV "–". Footnote: "*p < 0.05, **p < 0.01."

**Data-analysis checks (p. 9)**
- Accelerometer Euclidean distance: "there was no significant difference in the Euclidian distance neither during baseline nor during task times of interest (Kruskal-Wallis H = 4.24, p = 0.375, and H = 4.06, p = 0.398, respectively)".
- Heart-rate transformation: "The heart rate measure was not normally distributed (Shapiro-Wilk p < 0.001)… This resulted in normally distributed data appropriate for the parametric tests (Shapiro-Wilk p = 0.064)." (p. 9)

**Task performance (p. 9)**
- "The task completion time was affected by maintenance conditions (F = 10.263, p = 0.001)."
- "there was a statistically significant difference in the distribution of errors across maintenance conditions (χ2 = 20.201, p = 0.0001)."
- "only AH and AV led to statistically error reduction compared to the paper-based manual condition (p = 0.001 and p = 0.003, respectively)."
- "no statistical one-way interaction was observed between information and interaction mode (F = 0.160, p = 0.692) for task completion time."
- "there was a statistically significant main effect of information mode (F = 8.410, p = 0.006) but not on the interaction mode (F = 0.440, p = 0.512) toward task completion time."
- "The average completion time for 3D animation users was 501.1 s (SD = 83.60 s), and 583.1 s (SD = 93.73 s) for video users."
- "main effect of information mode (U = 129, p = 0.005) was also found for number of errors as indicated by mean rank (16.95 and 25.64 for 3D animation and video, respectively) but did not show the statistically significant main effect for interaction mode (U = 191, p = 0.509)."
- **Table 4 (p. 10), "Percentage improvement over paper-based maintenance":** Improvement over control group / Improvement Variability — VH 31.8 / 71.8; AH 42.7 / 75.1; VV 35.1 / 63.9; AV 43.1 / 68.3.
- Fig. 2 (p. 10) "Task completion time for different maintenance conditions. For an outlier in the data (23), log transformation was applied. *p < 0.05." — boxplot, y-axis "SECOND" 250.00–1500.00; no numeric data labels printed. Fig. 3 (p. 10) "Number of errors for different maintenance conditions. *p < 0.05." — no numeric data labels printed.
- Predicted ordering stated before results, p. 9: "Based on the number of resource allocation, participants will perform the best in the AV condition, and the least performance improvement over paper-based manual will be in the VH condition."

**Subjective workload — NASA-TLX (pp. 9–10)**
- Between AR and control: "Mental (χ2 = 9.96, p = 0.041) and Temporal workload (χ2 = 10.40, p = 0.034), but not in Physical (χ2 = 7.11, p = 0.13), Effort (χ2 = 5.75, p = 0.218), Performance (χ2 = 1.97, p = 0.741), Frustration (χ2 = 7.49, p = 0.112) and total workload component. (χ2 = 8.16, p = 0.086)." (p. 9)
- Information mode: "Mental (U = 232.5, p = 0.752), Physical (U = 242, p = 0.577), Temporal (U = 177, p = 0.273), Performance (U = 255, p = 0.369), Effort (U = 195, p = 0.523), Frustration (U = 224.5, p = 0.906), and Total (U = 236, p = 0.687)". (p. 9)
- Interaction mode: "Mental (U = 272.5, p = 0.188), Physical (U = 290, p = 0.079), Temporal (U = 281.5, p = 0.121), Performance (U = 217, p = 0.929), Effort (U = 267, p = 0.235), Frustration (U = 231, p = 0.782), and Total (U = 277.5, p = 0.151)." (pp. 9–10)
- Paired-samples t-test within CG (paper-based vs AR run-through), p. 10: "Mental (t = 3.272, p = 0.011), Physical (t = 2.828, p = 0.022), Effort (t = 4.316, p = 0.003) and Total (t = 4.035, p = 0.004), but not for Temporal (t = 1.335, p = 0.219), Performance (t = 1.892, p = 0.095), and Frustration (t = 1.348, p = 0.214)."
- p. 10: "For each group of maintenance condition, there was no association established between components of task performance and perceived workload components of NASA-TLX."
- Fig. 4 (p. 11) "Distribution of perceived workload across conditions." — no numeric data labels printed.

**Physiological data (pp. 10–11)**
- Sample, p. 10: "Physiological arousal data consisted of 46 participants, as due to technical issues, 5 participants did not have the complete physiological measures data sets. Each experimental group had 9 participants each, and control group had 10."
- Mixed ANOVA interactions, p. 10: "there was no one-way interaction between within- and between-subject factor for SCL, SCR, RMSSD, and HR (F = 1.530, p = 0.210, F = 0.44, p = 0.778, F = 0.14, p = 0.246, F = 2.45, p = 0.061, respectively)."
- Within-subject main effects, p. 10: "significant for SCL, SCR, and HR where task related physiological response was significantly higher than baseline (F = 100.77, p < 0.001, F = 169.85, p < 0.001, F = 131.56, p < 0.001 respectively), but not for RMSSD (F = 0.420, p = 0.520)."
- Between-subject main effects, pp. 10–11: "not statistically significant for SCL, SCR, RMSSD, and HR (F = 1.09, p = 0.373, F = 1.360, p = 0.264, F = 2.330, p = 0.073, F = 1.870, p = 0.135)."
- One-way MANOVA on difference scores, p. 11: "a significant result only for the difference in HR (F = 2.65, p = 0.047), but not for SCL, SCR, RMSSD (F = 1.6, p = 0.185, F = 0.18, p = 0.948, F = 1.42, p = 0.246). Post-hoc test revealed only a significant higher of HR for AH compared to CG (p = 0.048)."
- **Table 5 (p. 11), "Physiological response across conditions", M (SD) for Task / Baseline / Difference:** SCL — AH 0.57 (0.32) / −0.92 (0.38) / 1.49 (0.62); AV 0.46 (0.40) / −0.75 (0.61) / 1.22 (0.94); VH 0.39 (0.46) / −0.32 (0.66) / 0.71 (1.03); VV 0.65 (0.36) / −0.89 (0.65) / 1.54 (0.84); CG 0.36 (0.22) / −0.69 (0.45) / 1.08 (0.53). SCR — AH 0.46 (0.20) / −0.60 (0.20) / 1.06 (0.34); AV 0.35 (0.38) / −0.48 (0.38) / 0.83 (0.59); VH 0.43 (0.29) / −0.57 (0.29) / 1.01 (0.45); VV 0.38 (0.84) / −0.44 (0.18) / 0.82 (0.41); CG 0.29 (0.53) / −0.60 (0.27) / 0.92 (0.55). RMSSD — AH 0.33 (0.49) / 0.22 (0.66) / 0.11 (0.92); AV 0.49 (0.69) / 0.46 (0.56) / 0.16 (0.54); VH 0.21 (0.68) / 0.15 (0.20) / 0.06 (0.70); VV 0.46 (0.66) / 0.48 (0.30) / −0.18 (0.42); CG 0.67 (0.58) / 1.02 (0.87) / −0.49 (0.69). HR — AH 0.80 (0.34) / −0.93 (0.32) / 1.73 (0.55); AV 0.63 (0.45) / −0.94 (0.29) / 1.57 (0.70); VH 0.65 (0.29) / −0.59 (0.40) / 1.24 (0.64); VV 0.51 (0.54) / −0.72 (0.60) / 1.23 (1.05); CG 0.15 (0.29) / −0.54 (0.54) / 0.70 (0.77).

**Eye-tracking / gaze behaviour (pp. 11–13)**
- **Table 6 (pp. 11–12), "Eye-tracking data availability in each AR condition":** VH total 11, available 6, "YES"; VV total 10, available 4, "Insufficient residual for degree of freedom"; AH total 10, available 9, "YES"; AV total 10, available 10, "YES".
- VH condition, p. 11: "a statistically significant difference for total dwell time (F = 34.05, p = 0.001)"; post hoc: "video (V) and main-instruction (M)" p = 0.002; "video and sub-instruction (Sb) (p = 0.002)"; "video and tool (To) (p = 0.003)"; "gearbox (Gb) and main instruction (p = 0.015)"; continued p. 12: "gearbox and sub-instruction (p = 0.022), gearbox and tool (p = 0.021), but not between video and gearbox (p = 0.775)."
- VH fixations, p. 12: "total number of fixations (F = 35.44, p = 0.001)"; post hoc "video and main instruction (p = 0.005), video and sub-instruction (p = 0.016), video and tool (p = 0.009) as well as gearbox and main instruction (p = 0.006), gearbox and sub-instruction (p = 0.035), gearbox and tool (p = 0.012), but not between video and gearbox (p = 1.0)."
- VH proportions, p. 12: dwell "video (35.68%) and gearbox (55.18%) rather than textual instructions (5.64%) and 2D image (3.5%)"; glances "video (44.17%) and gearbox (33.25%)… textual information (12.32%) and 2D image (10.26%)".
- VH transitions, p. 12: "a significant difference in the number of transitions for highly transitioned area or fixations (F = 6.34, p = 0.001)"; "the transition counts between Gb-Gb and To-S were statistically significant different (p = 0.024)".
- AH condition, p. 12: "a statistically significant difference for total dwell time (F = 93.21, p = 0.001)"; "the total dwell time for Gb was statistically significantly higher than M (p = 0.009), S (p = 0.013), and To (p = 0.012)"; "a statistically significant difference for the number of fixations for different AOIs (F = 225.53, p = 0.001)"; "the number of fixations of Gb was statistically significantly higher than M (p = 0.0001), Gb and S (p = 0.0001), Gb and To (p = 0.0001)."
- AH proportions, p. 12: dwell "gearbox (i.e., animation, accounts for 85.3%) compared to textual instructions (11.08%) and 2D images (3.61%)"; fixations "gearbox (58.07%) than textual instruction (28.22%) and 2D image (13.7%)".
- AH transitions, p. 12: "a significant difference among transitions (F = 74.68, p = 0.0001)"; "Gb-Gb was statistically significantly higher than To-S (p = 0.003), S–S (p = 0.001), Gb-S (p = 0.0001), and Gb-To (p = 0.0001). Further, To-S and Gb-S was statistically significantly higher than Gb-To (p = 0.042 and 0.027, respectively)."
- VH vs AH, p. 12: "no statistically significant difference (F = 0.01, p = 0.925) between video + gearbox (90.86%) in VH and gearbox (85.3%) in AH"; "a statistically significant difference with respect to the number of fixations (F = 24.57, p = 0.001) between video + gearbox (77.42%) in VH and gearbox (58.07%) in AH".
- Fixation Rate, p. 13: "the analysis of FR elicited a statistically significantly high ratio of AOIs for video + gearbox in VH compared to AOI for gearbox in AH condition (F = 38.65, p = 0.0001)."
- Eye–performance correlations, p. 13: "The total number of fixations from the sum of video and gearbox fixation counts elicited by participants in the VH condition was statistically positively correlated with the time taken to complete the task (rs = 0.841, p = 0.036). Similarly, the total number of fixations on the gearbox elicited by participants in the AH condition positively correlated with task completion time (rs = 0.717, p = 0.03)."
- Sampling rates: eye tracker "30 Hz approximately, which results in 30 individual gaze points per second" (p. 7); fixation threshold "a minimum period of time during which the eye remains within a small area for 100 msec" (p. 7); EDA "The sampling rate of EDA was 4 Hz" (p. 7); accelerometer "3-axis accelerometer (sampling rate 32 Hz)" (p. 8); baseline "A 5-min rest position baseline" (p. 8).

**Perceived usability — PSSUQ (p. 13)**
- "no one-way interaction between information mode and input modality on the measure of system usefulness (F = 2.292, p = 0.139) and interface quality (F = 0.244, p = 0.625)."
- "both system usefulness and interface quality were not influenced by main effect of information mode (F = 2.814, p = 0.102 and F = 1.638, p = 0.209) and input modality (F = 1.866, p = 0.178 and F = 0.066, p = 0.799)."
- **Table 7 (p. 13), "Two-way MANOVA analysis" (System usefulness & Interface Quality):** IM "F = 1.454 (p = 0.247)", "Wilks' Λ = .925"; II "F = 1.652 (p = 0.206)", "Wilks' Λ = .916"; IM × II "F = 1.201 (p = 0.313)", "Wilks' Λ = .937".

**Subjective assessment counts (pp. 13–14)**
- "almost all (20 out of 21) participants said that the video instruction was simple, easy to follow, intuitive, and effective" (p. 13).
- "all participants in the animation group (20 out of 20) found that the animation-based visualisation is easy to understand" (p. 14); "Only three participants noted the misalignment of the animation overlaid on the machine" (p. 14).
- "all participants (21 out of 21) liked the hand gesture to navigate through the instructions" (p. 14).
- "All participants in the voice group (20 out of 20) said they liked voice command as an interaction modality." (p. 14)
- "almost all AR participants (38 out of 41) said that the Microsoft Hololens 2 was comfortable and not too heavy. But, since they were using it only for 7–8 min on average…" (p. 14)

**Discussion figure (p. 14):** "the 3D animation-based instruction show a significantly faster completion time (14%) compared to a video-based visualisation" (also abstract, p. 1).

**Error coding scheme (p. 6):** "Errors were recorded when the participant misinterpreted the given instructions, which were coded in five categories for ease of data collection including: 0-no error, 1-wrong tool, 2-wrong tool movement, 3-wrong tool orientation, 4-wrong part orientation, 5-other error."

**Statistical significance threshold (p. 9):** "SPSS v26 (IBM, 2019) was used to perform statistical analysis with statistical significance determined when p < 0.05, when not changed by the Bonferroni correction."

### What was measured beyond time and errors
Named exactly as the paper names them:
- **"NASA-TLX (Hart and Staveland, 1988), which consists of six workload components (Mental, Physical, Temporal, Performance, Effort, and Frustration)… The self-rating score ranges from 0 to 100 and a raw score was computed."** (p. 6)
- **"Empatica E4 wristband (Garbarino et al., 2014) was used to capture continuous physiological measures of electrodermal activity (EDA) and interbeat interval (IBI) to measure levels of autonomic arousal."** — decomposed into "skin conductance level (SCL) and skin conductance response (SCR) using Ledalab"; "IBI data was used to calculate heart rate (HR) and heart rate variability measure represented by RMSSD via Kubios (Niskanen et al., 2004)". (p. 7)
- **Accelerometer:** "The wristband also collected accelerometer data on a 3-axis accelerometer (sampling rate 32 Hz), which was later used to monitor the wrist movement of the participants." (p. 8); "Accelerometer data was used to calculate Euclidian distance between x, y and z coordinates" (p. 9).
- **"Post-Study System Usability Questionnaire (PSSUQ) (Lewis, 1992) was used to measure perceived satisfaction on three sub-scales, namely system usefulness, information quality, and interface quality."** — administered on two only: "the developed AR system was evaluated with respect to the system usefulness (6 questions) and interface quality (3 questions)." (p. 7)
- **"Subjective assessment consisted of four questions administered to capture the participants' opinions concerning the usability of the AR system."** (p. 7)
- **Eye tracking:** "An eye-tracker on the Microsoft Hololens 2 was used to record eye movements during user interaction with the AR contents or Area of Interests (AOIs)." Metrics: "dwell time, number of fixations, and transition between AOIs or fixations within one AOI"; plus "another metric, namely Fixation Rate (FR)… defined as the number of fixations on specific AOIs divided by the total number of fixations on the Area of Glance (AOG)". (p. 7)
- **Performance indexes:** "The quality of performance was assessed in terms of completion time and the number of errors." (p. 6)

### How the system decides what to show the user
The overlay content is pre-authored and stepwise; tracking/registration is marker-less object recognition; no adaptive or data-driven selection is claimed.
- p. 6: "Vuforia version 8.3.8 object tracking (PTC, 2019) was used for the AR tracking library and registration. The virtual content management, user interface, and data collections were programmed in Unity version 2019.1.10f (Unity, 2019) and interfaced with a local web and database server."
- p. 8: "some other groups of participants were given the AR system that guided them step by step through the completion of the maintenance task. The AR system consisted of textual instructions that displayed the maintenance problem and the procedure to fix the problem. It also displayed the picture of the tool required for each step and additional textual information describing the name and the tool specification (visual/cognitive and verbal). In this case, searching task was eliminated in AR conditions."
- p. 8: "1) processing video-based visualisation that showed footage of an expert using the tool to carry out the assembly/disassembly of the gearbox component step by step… and 2) processing animation-based visualisation that overlaid virtual instructions and components on the top of the gearbox and the animated assembly and disassembly process right where it should be done on the gearbox… In this case, locating was eliminated in the animation-based visualisation. Both visualisations were set to always-on mode rather than on-request as suggested in previous research (Kim et al., 2019)."
- p. 8: "the AR system was also designed to have two distinct interaction modalities to navigate through the virtual instructions such as: 1) Hand gesture interaction (requires visual modality) which allows the user to use press buttons (responding and spatial) in the air, and 2) Voice command which allows the user to say some keywords such as 'Back' or 'Next' to control the instructions (responding and verbal)."
- pp. 8–9: "AR makes the maintenance task easier for the user because task-relevant information is presented directly to the user's viewpoint right when and where it is needed".
- Tracking failures affecting what was displayed, p. 6: "The technical errors were related to the limitation of AR tracking library that failed to recognise the object target which result in AR content not or properly overlaid in the real environment. The recognition errors could be due the physical object that being used consists of moving parts, surface with poor texture, and a limited number of contrast-based features (PTC, 2019). As a result, some participants had trouble to see AR contents to perform the task."
- No adaptive/decision logic, p. 16: "However, the current system implementation was not equipped with the decision support capabilities necessary to provide adaptive system support."

### Authors' own limitations and future work
- **Section "6.6. Limitations" (p. 16):** "Firstly, the maintenance tasks tested in this study were relatively simple and limited, consisting of the use of an Allen key to (un)screw bolts and simple hand movements to perform the assembly and disassembly process. It is therefore questionable whether this result could be generalised for a more complex assembly task such as an aircraft cable assembly task." / "In the current study, a slight misalignment of 3D animation was still comprehensible for the AR users, but the question arises whether this would still be the case with assembly tasks that require more precision in terms of tracking and registration of virtual objects to real components such as in spacecraft cable assembly. Further research should investigate the effect of imprecise overlay of AR on complex assembly tasks and the optimal type of information mode." / "Secondly, the task duration and environment considered in the present study may not be sufficient to reflect a real industrial setting that can be noisy, with poor lighting as well as exposure to time pressure. These factors may have impact on the efficacy of information and interaction mode."
- **In-text limitations elsewhere:** p. 6 exclusions and tracking failures (quoted above); p. 9 "there remains unclear as to why correlations in VH and AV were observed between performance (i.e. CT and errors) and age as well as maintenance experience"; p. 11 "due to unforeseen technical issues, the server failed to log eye movement data from some participants"; p. 11 "Since the number of samples of VV minus the number of groups is equal to zero, it results in insufficient residual for degree of freedom for conducting an analysis of variance"; p. 13 "the use of PSSUQ was not suitable for the control group due to the absence of an interface"; p. 14 "since they were using it only for 7–8 min on average, most of them questioned if it would be comfortable for prolonged use"; p. 16 "the current system implementation was not equipped with the decision support capabilities necessary to provide adaptive system support."
- **Section 7 "Conclusion and further research" (p. 17):** "Future research should investigate some other usability issues that maximise efficient visual scanning behaviour, address over-reliance on AR information, and other ergonomic issues that might put the users at risk of occupational hazards." / "It also important to address the use of AR for a wide range of users including those who experience vision distance related problems and slow hand-eye coordination to ensure inclusive implementation of AR in the long term." / "From the technological side, the AR tracking and registration were still not without flaws even in the laboratory setting. Therefore, the robustness of AR tracking and registration is also critical and should be improved, especially for the implementation in the industrial contexts where the environment is relatively more complex." / "It is also questionable to what extent the design practices are transferable across different applications (e.g. medical)."
- p. 15: "further research is needed to test for a prolonged usage (Wang et al., 2019)."

### Five verbatim headline claims
1. "The results show that different information modes have distinct impacts compared to paper-based maintenance, in particular, 3D animation led to a 14% improvement over the video instructions in task completion time." (p. 1)
2. "Moreover, insights from eye gaze behaviours such as number of fixations and transition between Areas of Interest (AOIs) revealed the differences in attention switching and task comprehension difficulty with the choice of AR modalities." (p. 1)
3. "Taken all this together, this study found that the use of AR can reduce task-related workload (higher resource allocation for searching and interpreting according to MRM in Table 2), which is evident from NASA-TLX, subjective perception, and performance measures." (p. 15)
4. "The findings show that 3D animation could help to achieve faster task completion times and fewer errors in guiding task assembly compared to video-based instructions." (p. 17)
5. "Nevertheless, the use of 3D animation could cause confusion to the users and possibly, provide misguided information when the registration is misaligned, and the brightness and occlusion are not appropriately designed." (p. 17)

### Theoretical models and frameworks invoked (named exactly)
- **Multiple Resource Model (MRM)** — p. 2: "This paper aims to provide a theoretically driven approach based on the Multiple Resource Model (MRM) (Wickens, 2002, 2008)…"; p. 2 (Section "2. Theoretical background"): "The multiple resource model (MRM) proposes multiple attention resources that account for variance in time-sharing performance (Wickens, 2002, 2008). The MRM posits that attentional resources are limited, and their structure can be described by four categorical in which each dimension has two discrete levels: processing stages (visual/cognitive and responding), processing codes (verbal and spatial), input modality (visual and auditory), and responses (manual and verbal)." Operationalised in **Table 2 (p. 6), "The distribution of resources allocation according to MRM in different maintenance conditions"**; used again pp. 14, 15, 16, 17.
- **Stimulus-Central Processing-Response (S–C-R) compatibility model / "resources compatibility concept"** — p. 2: "Wickens et al. (1983) propose the concept of Stimulus-Central Processing-Response (S–C-R) compatibility model that predicts the performance variations based on compatibility relation between modalities of input (auditory, visual or A, V) and output (manual, speech, or M, S), and codes of central processing (spatial versus verbal)."; p. 16: "according to the resource compatibility concept, performance benefits are better realised when the visual/manual modalities are associated with spatial encoding (animation) than when the spatial is paired with verbal encoding (text + video)."
- **Joint Cognitive System (JCS)** — p. 1: "which must be considered and addressed from the joint cognitive systems' (JCSs) perspective"; p. 2: "JCS is characterised by three principles (Hollnagel, 2005): (a) support for coping, (b) time management, and (c) predictability."; Section "6.5. Discussion on designing AR and the user as a joint cognitive system" (p. 16).
- Supporting eye-tracking interpretive constructs named on p. 7: dwell time as "an indicator for the level of visual engagement with the AOIs (Tullis and Albert, 2008)"; number of fixations as "the level of importance of an AOI (Poole et al., 2007) or inefficient information processing (Goldberg and Kotval, 1999)"; Fixation Rate as "an indicator of information decoding complexity for comprehension tasks (Sharafi et al., 2015)".

### Internal inconsistencies
1. **Whether AR conditions differed in completion time.** p. 9: "A post hoc test revealed that the completion time in all AR conditions was significantly lower than the control group as indicated in Fig. 2. However, there was no completion time difference observed between the different AR conditions." Against, same page: "there was a statistically significant main effect of information mode (F = 8.410, p = 0.006)… The average completion time for 3D animation users was 501.1 s (SD = 83.60 s), and 583.1 s (SD = 93.73 s) for video users."; p. 14: "the 3D animation-based instruction show a significantly faster completion time (14%) compared to a video-based visualisation"; p. 1: "3D animation led to a 14% improvement over the video instructions in task completion time."
2. **Direction of the fixation-count / performance relationship.** p. 13: "The total number of fixations… in the VH condition was statistically positively correlated with the time taken to complete the task (rs = 0.841, p = 0.036)… This implies that the more AR modality could facilitate users to integrate required information for task completion within the vicinity (higher number of fixations in the specific AOI, 77.42% (video + gearbox) in VH and 58.07% (gearbox) in AH), the better the task performance would be achieved in terms of task completion time." Against, p. 16: "the significant correlation between the total number of fixations and the completion time in both conditions revealed that fixations frequency could be a good predictor of task completion time, namely those who gazed more often at the dynamic graphical instructions tend to spend more time on completing the task." (Also, the two AOI percentages cited on p. 13 as "higher number of fixations" are 77.42% for the slower VH condition and 58.07% for the faster AH condition.)
3. **Whether workload differed between AR and paper.** p. 9: "there was a statistically significant difference in subjective workload distribution between AR and control group conditions as assessed by NASA-TLX measure in Mental (χ2 = 9.96, p = 0.041) and Temporal workload (χ2 = 10.40, p = 0.034)… Following the statistically significant results, post hoc comparison did not reveal any statistically significant difference across maintenance conditions for Mental and Temporal workload." Against, p. 14: "the measure of subjective workload did not reveal any statistical evidence on the account of workload difference."
4. **Error coding: "five categories" but six codes listed.** p. 6: "which were coded in five categories for ease of data collection including: 0-no error, 1-wrong tool, 2-wrong tool movement, 3-wrong tool orientation, 4-wrong part orientation, 5-other error."
5. **A paragraph is printed twice on p. 3 with divergent endings.** First: "In regard to perceiving virtual and real information simultaneously, one study examined the negative effect of context switching (i.e. real to AR view) on user perception and eye strain wearing a monocular optical HMD (Gabbard et al., 2019). Since the object of interest in the real world can occur at a range of distances… Tested under a text-based visual search, context switching had a negative impact on performance when information was presented at the far distance (6 m)…". Second, same page: "In regard to perceiving virtual and real information simultaneously, one study examined the negative effect of context switching (i.e. real to AR view) on user perception and eye strain wearing a monocular optical HMD (Gabbard et al., 2019). Since an object of interest in the real world can occur at a range of distances… Their study found a significant effect of focal distance switching on task performance."
6. **Fig. 5 and Fig. 6 carry identical captions.** Fig. 5 (p. 12): "Eye gaze behaviours of Video-Hand gesture (VH) and Video-Voice command (VV): a) Dwell time, b) Number of Fixations, and c) transition between AOIs or fixations." Fig. 6 (p. 13): "Eye gaze behaviours of Video-Hand gesture (VH) and Video-Voice command (VV): a) Dwell time, b) Number of Fixations, and c) transition between AOIs or fixations." The text on p. 11 cites both for a comparison across information modes: "Fig. 5 and Fig. 6 show similar distributions between interaction mode across information mode samples for total dwell time, number of fixations, and transition between AOIs"; no caption names the animation conditions (AH, AV).
7. **A reported F/p pair that does not correspond.** p. 10, mixed-ANOVA interaction for RMSSD: "F = 0.14, p = 0.246" (an F below 1 reported with p = 0.246); the same p value is reported again on p. 11 for the RMSSD MANOVA with a different F: "F = 1.42, p = 0.246".
8. **Post hoc transition results restated differently.** p. 12 (results): "Gb-Gb was statistically significantly higher than To-S (p = 0.003), S–S (p = 0.001), Gb-S (p = 0.0001), and Gb-To (p = 0.0001)." p. 16 (discussion): "there was a significant difference between Gearbox-Gearbox and Gearbox-SubInstruction, Gearbox-Tool, and Gearbox-Tool in animation-based instruction" — "Gearbox-Tool" is listed twice and To-S and S–S are omitted.
9. **The excluded-participant count versus the analysed N.** p. 6 reports "sixty-three volunteers", nine excluded for incomplete data and three for age; the resulting analysed total is never stated in text. Table 3 (p. 7) lists 10 + 11 + 10 + 10 + 10; p. 10 states "Physiological arousal data consisted of 46 participants, as due to technical issues, 5 participants did not have the complete physiological measures data sets."agentId: a1bb1c8a2e9f8d47f (use SendMessage with to: 'a1bb1c8a2e9f8d47f', summary: '<5-10 word recap>' to continue this agent)
<usage>subagent_tokens: 153816
tool_uses: 28
duration_ms: 471046</usage>
---

# PART 3 — Daling, Farr, Gonzalez-Franco

I've extracted and read all three papers page by page (including rendering figures where numbers appeared only in graphics). Below is the evidence profile.

---

# Paper 1 — `1-s2.0-S0003687023000595-main.pdf` (12 PDF pages)

## Citation as printed
- **Authors:** Lea M. Daling, Marisa Tenbrock, Ingrid Isenhardt, Sabine J. Schlittmeier (p. 1)
- **Year:** 2023 (p. 1)
- **Title:** "Assemble it like this! – Is AR- or VR-based training an effective alternative to video-based training in manual assembly?" (p. 1)
- **Journal:** Applied Ergonomics (p. 1)
- **Volume:** 110 (p. 1, running header "Applied Ergonomics 110 (2023) 104021")
- **Issue:** not printed
- **Page range:** not printed. Article number 104021 (p. 1). The article's own internal pagination runs 1–12 (footers, pp. 1–12 of the PDF).
- **DOI:** https://doi.org/10.1016/j.apergo.2023.104021 (p. 1)
- Additional dates as printed: "Received 25 July 2022; Received in revised form 15 March 2023; Accepted 17 March 2023" and "Available online 30 March 2023" (p. 1). Open access, CC BY-NC-ND (p. 1).

## Type of work, in the paper's own words
No article-type label is printed beyond "A R T I C L E I N F O" (p. 1). Self-descriptions:
- "In this study, we investigated and compared the effects of AR-, VR- and video-based training on short- and long-term objective performance measures and subjective evaluations in a manual assembly task." (p. 1)
- "we used a 3× 2 repeated-measures experimental design" (p. 3)
- "the authors performed another post hoc exploratory analysis of the data" (p. 3)

## What was studied
- **Task:** manual assembly of a LEGO® MINDSTORMS® EV3 robot, "which consisted of ten steps" (p. 3); 14 components in open storage boxes (p. 3); "The assembly was deemed to be completed when the LEGO® MINDSTORMS® EV3 robot was put into operation." (p. 5)
- **Devices:** AR group — "Microsoft HoloLens 1 HMD", operated "with either speech commands, hand gestures or the clicker", Unity 2019.4.3f1 with Microsoft Mixed Reality Toolkit, physical LEGO components (p. 4). VR group — "Oculus Rift VR HMD and controllers", Auto Hand® VR Physics Interaction, Unity 2019.4.3f1, virtual components (p. 4). Video group — "Samsung tablet (10.4 inches) and a touch pen", physical LEGO components, "video clips of real hands performing the task" (p. 4).
- **Conditions compared:** AR-based vs VR-based vs video-based training; "The training slides of the instruction protocol were identical for all groups, while the training medium differed" (p. 3).
- **Recruited:** "In total, N = 103 participants took part in the study and were randomly allocated into three experimental groups as follows: both the video group and AR group had 34 participants, while the VR group consisted of 35 participants." (p. 3)
- **Exclusions and stated reason:** "However, due to technical problems with the VR application, seven participants in the VR group were excluded from the analysis, so that this group finally entered data analysis with 28 participants." (p. 3)
- **Analysed:** "Of the 96 participants included in the analyses, 51 were male (53.1%) and 45 were female (46.9%). The mean age was 30.19 years, with a standard deviation of 11.46 years, a minimum age of 18 years, and a maximum age of 73 years." (p. 3)
- **Further pairwise exclusions for outliers:** "An outlier analysis was conducted for all variables on the group level. Z scores were inspected for outliers above three standard deviations, resulting in the pairwise exclusion of identified cases from the analysis." (p. 5, and repeated p. 6). Resulting Ns: TCT — AR 33, VR 27, video 33 (Table 1, p. 5; Fig. 3 note, p. 6); error count — AR 34, VR 27, video 33 (Table 1, p. 5; Fig. 4 note, p. 7); NASA-TLX and SUS — AR 34, VR 28, video 34 (Table 2, p. 5; Fig. 5 and Fig. 6 notes, p. 7).
- **Recruitment and consent:** "Participants were recruited via word of mouth, mailing lists, social networks, email, and posters. Participation was voluntary and participants did not receive any payment or reward." (pp. 3–4) "The inclusion criterion was a minimum age of 18 years and informed consent was provided before the experiment." (p. 4) Ethics approval number 2021_02_FB7_RWTH AACHEN (p. 4).
- **A priori power analysis:** "An a priori power analysis for a repeated measures ANOVA with within-between interaction using G*Power (Faul et al., 2007) determined a required total sample size of N = 81 for a power of 0.8 and a medium effect (Cohen's f) of 0.25 (Cohen, 1988) with a significance level of 5%." (p. 4)
- **Prior experience as printed (p. 4):** video group "26.5% (nine participants) reported no prior experience with video tutorials, and 73.5% (25 participants) indicated having used video tutorials before"; VR group "40.7% (eleven participants) reported having no experience using VR, and 59.3% (16 participants) reported having used VR before"; AR group "62.5% (20 participants) indicated having no experience using AR, and 37.6% (12 participants) reported prior experience". Occupational status: "53.1% (51 participants) reported having student status, 49% (47 participants) reported being employed (multiple selections were possible), and 4.2% (four participants) were retired." (p. 4)

## The testing schedule
- "Training success was evaluated both immediately after training (T1) and two weeks later (T2)." (p. 3)
- "TCT and error count were used as outcome variables in an assembly task that was performed twice: immediately after training (T1) and two weeks after training (T2)." (p. 3)
- "The experiment consisted of two single sessions for all participants. T1 consisted of the training and the assembly: the procedure took 75–100 min. T2 took place two weeks later, with participants performing the assembly again with no training provided: the procedure took 15–35 min." (p. 4)
- "The second experimental session took place two weeks after the first to measure long-term effects of training. For T2, participants were asked to perform the assembly as quickly and accurately as possible from their memory." (p. 5)
- Order within T1: "After the training phase, participants filled in the NASA-TLX and the SUS. Then they performed the assembly." (p. 5) Fig. 2 (p. 5) labels the gap between T1 and T2 as "2-week interval".
- Rationale as printed: "The two-week interval is the current standard in the literature for long-term retention of industry training (Carlson et al., 2015; Murcia-López and Steed, 2018)" (p. 3).

## Every quantitative result

**Descriptive — TCT and error count (Table 1, p. 5).** TCT in minutes; SE = standard error.
| Group | Measure | T1: N, Min, Max, M, SE, 95%-CI | T2: N, Min, Max, M, SE, 95%-CI |
|---|---|---|---|
| AR | TCT | 33, 3.52, 8.47, 5.1, .24, [4.62, 5.58] | 33, 6.01, 15.51, 9.79, .48, [8.80, 10.79] |
| AR | Error count | 34, 0, 10, 3.29, .45, [2.37, 4.21] | 34, 1, 16, 7.06, .68, [2.86, 4.55] |
| VR | TCT | 27, 4, 9.95, 5.74, .29, [5.13, 6.36] | 27, 5.27, 15.73, 9.29, .52, [5.67, 8.45] |
| VR | Error count | 27, 0, 9, 3.7, .41, [2.86, 4.55] | 27, 1, 15, 9.15, .61, [7.88, 10.41] |
| Video | TCT | 33, 3.07, 11.43, 5.13, .28, [4.55, 5.70] | 33, 5.49, 15.97, 10.02, .51, [8.98, 11.05] |
| Video | Error count | 33, 0, 8, 3.55, .36, [2.81, 4.28] | 33, 1, 16, 7.59, .62, [6.33, 8.85] |

**Descriptive — subjective evaluations (Table 2, p. 5).**
| Group | Measure | N | Min | Max | M | SE | 95%-CI |
|---|---|---|---|---|---|---|---|
| AR | NASA TLX | 34 | 17.17 | 57.17 | 35.75 | −1.85 | [31.98, 39.51] |
| AR | SUS | 34 | 62.5 | 100.0 | 81.76 | 1.78 | [78.14, 85.39] |
| VR | NASA TLX | 28 | 27.83 | 73.67 | 52.29 | 2.03 | [48.13, 56.45] |
| VR | SUS | 28 | 25.0 | 85.0 | 61.07 | 3.2 | [54.52, 67.63] |
| Video | NASA TLX | 34 | 16.5 | 64.5 | 40.25 | 2.11 | [35.95, 44.55] |
| Video | SUS | 34 | 62.5 | 100.0 | 85.96 | 1.58 | [82.76, 89.76] |

**TCT, 3 × 2 ANOVA (p. 6).**
- Main effect of time of measurement: "F(1, 90) = 262.99, p ≤.001, η2p = .75"
- "On T1, overall mean TCT was M = 5.32 min, SE = .16, 95%-CI [5.01, 5.64], and on T2 overall mean TCT was M = 9.70 min, SE = .29, 95%-CI [9.12, 10.29], with a mean difference of MDiff = 4.38 min, SE = .27, 95%-CI [3.84, 4.92] between T1 and T2."
- Main effect of training group: "F(2, 90) = 2.52, p = .964"
- Interaction time × training group: "F(2, 90) = 2.52, p = .111"

**Error count, 3 × 2 ANOVA (p. 6).**
- Main effect of time of measurement: "F(1, 91) = 144.95, p ≤.001, η2p = .61"
- "an overall mean of M = 3.51 errors, SE = .24, 95%-CI [3.04, 3.99] on T1 and an overall mean of M = 7.93 errors, SE = 0.38, 95%-CI [7.19, 8.68] on T2. Across all groups, there was a mean difference of MDiff = 4.42 errors between T1 and T2, SE = .37, 95%-CI [3.69, 5.147]."
- Main effect of training group: "F(2, 91) = 1.975, p = .145"
- Interaction: "F(2, 91) = 1.88, p = .159"
- Error coding as printed: "For severe errors, two errors were counted, for minor errors, one error was counted." "Using the manual was counted with one error point. The minimum error rate was zero, while a maximum of 68 errors were possible." (p. 6)

**NASA-TLX, one-way ANOVA (p. 6).**
- Reliability: "Cronbach's α = .66"
- "F(2, 93) = 17.06, p ≤.001, η2p = .27"
- Tukey post hoc: AR vs video "MDiff = −4.49, SE = 2.75, 95%-CI [−11.05, 2.06], p = .317"; VR vs video "MDiff = 12.04, SE = 2.89, 95%-CI [5.14, 18.94], p ≤.001"; VR vs AR "MDiff = 16.54, SE = 2.89, 95%-CI [9.64, 23.44], p ≤.001"

**SUS (pp. 6–7).**
- Reliability: positively formulated items "Cronbach's α = .78"; inverse items "Cronbach's α = 0.69" (p. 6)
- "Levene's Test showed that variances were not equal (p < .001)" (p. 6)
- "Welch's F(2, 55.7) = 24.17, p ≤.001, η2p = .43" (p. 7)
- Games-Howell: VR vs AR "MDiff = −20.7, SE = 3.66, 95%-CI [−29.57, −11.81], p ≤ .001"; VR vs video "MDiff = −24.88, SE = 3.56, 95%-CI [−33.55, −16.21], p ≤ .001"; AR vs video "MDiff = −4.19, SE = 2.37, 95%-CI [−9.88, 1.51], p = .190" (p. 7)
- Interpretation bands as printed: "A score of 60–80 indicates acceptable usability, a score above 80 indicates good to very good usability, and a score of 100 indicates excellent usability (Brooke, 1996)." (p. 6)

**Exploratory — training duration, one-way ANOVA (p. 8).**
- "F(2, 92) = 39.01, p < .001, η2p = .46"
- "The training duration was lowest in the AR group, M = 22.94 min, SE = .93, intermediate in the video group, M = 24.39 min, SE = .77, and highest in the VR group, M = 34.49 min, SE = 1.24."
- Tukey: video vs VR "MDiff = −10.09 min, SE = 1.41, 95%-CI [−13.46, −6.73], p ≤.001"; VR vs AR "MDiff = 11.55 min, SE = 1.40, 95% CI [8.21, 14.89] p ≤.001"; video vs AR "(MDiff = 1.46 min, SE = 1.34, 95% CI [−1.74, 4.66], p = .524)"

**Exploratory — age (p. 8).**
- Age did not differ between groups: "Welch's F(2, 59.28) = 1.5, p = .232"
- Age as covariate: TCT T1 "F(1,89) = 12.89, p = .001, η2p = .13"; TCT T2 "F(1,90) = 4.52, p = .036, η2p = .05"; error count T2 "F(1,91) = 6.53, p = .012, η2p = .07"; error count T1 "F(1,89) = 3.82, p = .054"
- Slopes: "b = .049, 95%-CI [.02,.07] for TCT on T1, b = .057, 95%-CI [.00,.11] for TCT on T2, b = .082, 95%-CI [.02,.15] for error count on T2"
- Group effect on TCT T1 adjusted for age: "F(2, 89) = 6.36, p < .05, η2p = .66"; Bonferroni post hoc VR vs AR "MDiff = .89 s, SE = .37, 95%-CI [.16, 1.64], p < .05"; VR vs video "(MDiff = .69 s, SE = .37, 95%-CI [-.03, 1.42], p = .062)"; AR vs video "(MDiff = −0.21 s, SE = .35, 95%-CI [-.91, .49], p = .056)"
- Group effect on error count T2 adjusted for age: "F(2, 91) = 3.52, p < .05, η2p = .07"; VR vs AR "MDiff = 2.38 errors, SE = .92, 95%-CI [−4.620, −.15], p < .05"; VR vs video "(MDiff = 1.66 errors, SE = .90, 95%-CI [-.54, 3.86], p = .206)"; AR vs video "(MDiff = −.73 errors, SE = .86, 95%-CI [−2.83, 1.38], p = 1.00)"
- Non-significant after adjusting for age: TCT T2 "F(2, 90) = .78, p = .460"; error count T1 "F(2, 89) = .43, p = .650"

**Exploratory — prior experience (p. 8).**
- Prior experience did not differ between groups: "F(2, 90) = 2.01, p = .140"
- Covariate for TCT T1: "F(1,87) = 4.52, p = .036, η2p = .05"; slope "b = −.51, 95%-CI [-.99, −.03]"
- Group effect on TCT T1 controlling for prior experience: "F(2,87) = 1.59, p = .209"
- Prior experience not a significant covariate for: TCT T2 "F(1,88) = 2.31, p = .133"; error count T1 "F(1,87) = 1.51, p = .222"; error count T2 "F(1,89) = 1.1, p = .299"

## Direction of each non-significant difference
- **TCT, main effect of training group (p = .964, p. 6).** T1 group means (Table 1, p. 5): AR 5.1 lowest, video 5.13, VR 5.74 highest. T2: VR 9.29 lowest, AR 9.79, video 10.02 highest.
- **TCT interaction (p = .111, p. 6).** Directions as above per time point (Table 1, p. 5).
- **Error count, main effect of training group (p = .145) and interaction (p = .159), p. 6.** T1: AR 3.29 lowest, video 3.55, VR 3.70 highest. T2: AR 7.06 lowest, video 7.59, VR 9.15 highest (Table 1, p. 5).
- **NASA-TLX, AR vs video (p = .317, p. 6).** MDiff = −4.49; AR numerically lower task load (AR M = 35.75 vs video M = 40.25, Table 2, p. 5).
- **SUS, AR vs video (p = .190, p. 7).** MDiff = −4.19; video numerically higher usability (video M = 85.96 vs AR M = 81.76, Table 2, p. 5).
- **Training duration, video vs AR (p = .524, p. 8).** MDiff = 1.46 min; AR numerically shorter (AR M = 22.94 vs video M = 24.39).
- **Age-adjusted TCT T1, VR vs video (p = .062, p. 8).** MDiff = .69; VR numerically higher (longer).
- **Age-adjusted TCT T1, AR vs video (p = .056, p. 8).** MDiff = −0.21; AR numerically lower (faster).
- **Age-adjusted error count T2, VR vs video (p = .206, p. 8).** MDiff = 1.66; VR numerically more errors.
- **Age-adjusted error count T2, AR vs video (p = 1.00, p. 8).** MDiff = −.73; AR numerically fewer errors.
- **Age-adjusted TCT T2 (p = .460) and error count T1 (p = .650), p. 8.** No pairwise directions are printed; Table 4 (p. 9) marks these rows "–", with the note "Empty rows (−) indicate that no effect of the covariate could be found."
- **Error count T1 covariate age (p = .054, p. 8).** Direction of the age slope for error count T1 is not printed.
- **TCT T1 controlling for prior experience, group effect (p = .209, p. 8).** Table 4 (p. 9) records "not different from each other"; no group means adjusted for prior experience are printed.
- **Age between groups (p = .232, p. 8) and prior experience between groups (p = .140, p. 8).** Group-level age means and experience means are not printed; group experience percentages are on p. 4.

## Authors' own stated limitations and future work
- p. 8: "The decrease in performance over time could be either due to the high complexity of the assembly steps or because participants were not explicitly informed that they were supposed to perform the assembly again from memory after two weeks. Further research should explore which factors contribute to performance decline and how the effect of forgetting can be minimized in all training methods."
- p. 9: "In our study, no differentiation of errors into solved vs. unsolved errors has been realized, which could have contributed to a better understanding of the effects of AR-vs. VR-based training on error count in assembly tasks."
- p. 9: "Whether increasing prior experience can be expected to have a positive effect on training success for all training methods should be clarified in the context of further research."
- pp. 9–10 (bridging p. 9 → p. 10 in the running text on p. 9): "However, since this analysis was exploratory and neither study design nor sample selection were aimed at these aspects, interpreting these results should be treated with caution. Further research is needed to uncover specific differences between and advantages of the training methods." (p. 9)
- p. 9: "Moreover, task load was only measured by using subjective ratings, which could limit the expressiveness of the results."
- p. 10: "Thus, the inclusion of additional objective data sources to assess task load in AR and VR training is recommended for future studies."
- p. 10: "Additional interviewing of participants after training about the reasons for their perceptions, e.g., via a retrospective think-aloud session, would be useful for future studies to further elucidate the reasons for low SUS in the VR condition."
- p. 10: "The disadvantages of VR compared to AR and video in terms of subjective evaluations and longer training durations should be addressed and investigated in further studies, e.g., through technological improvements of interaction features."
- p. 10: "Future research should thus investigate the use of these technologies at different age and expertise levels to better determine whether one of the technologies is more suitable for certain user groups."
- p. 10: "In all training groups there was a significant decline in performance over time. This raises the need for an investigation of factors influencing long-term skill retention."
- p. 10: "Future research should focus on the extent to which VR can further improve its usability and on its impact on perceived task load. In addition, the possible impact of subjective evaluations of the training methods on objective performance measures should be explored in further studies."
- p. 10: "In future research, associating objective performance measures and subjective evaluations should be an integral part of validating the conclusions found here comparing AR-, VR- and other conventional training in terms of their short- and long-term effects."
- p. 9 (on task complexity): "Both Gavish et al. (2015) and Liu et al. (2022) further discussed that the similarity of the training methods might be related to relatively easy assembly tasks and hypothesized that AR and VR might have a significant advantage over conventional methods when a more demanding task is used. This, however, cannot be supported by our study results."

## Five verbatim headline claims
1. "Our results showed that there was no difference between AR-, VR- and video-based training concerning the objective performance measures task completion time and error count." (p. 1)
2. "However, in the subjective evaluations VR-based training showed a significantly higher perceived task load and a lower usability rating than the AR- and video-based training regimes." (p. 1)
3. "An exploratory analysis additionally revealed partially better results for AR than for VR after adjusting the data for the age of the participants." (p. 1)
4. "Summarizing the analysis regarding the a priori hypotheses, it can be stated that there was no difference between the AR, VR, and video groups in their effect on short- and long-term objective performance measures, but TCT and error count significantly increased from T1 to T2 in all groups (RQ1)." (p. 7)
5. "Taking into account the effects on both objective performance and subjective evaluations, AR-based training in particular can be considered an effective alternative to video-based training to ensure short- and long-term training success in manual assembly tasks." (p. 10)

## Internal inconsistencies
- **Abstract vs participants section on sample size:** none found. The abstract (p. 1) prints no sample size, so there is nothing to disagree with the participants section.
- **Group sizes implied by the prior-experience percentages vs the stated group sizes.** Participants section, p. 3: "both the video group and AR group had 34 participants" and the VR group "finally entered data analysis with 28 participants". Prior-experience figures, p. 4: "In the VR group, 40.7% (eleven participants) reported having no experience using VR, and 59.3% (16 participants) reported having used VR before." (11 + 16 = 27, not 28). "In the AR group, 62.5% (20 participants) indicated having no experience using AR, and 37.6% (12 participants) reported prior experience." (20 + 12 = 32, not 34; and 62.5% + 37.6% = 100.1%).
- **Same F statistic reported for two different effects with two different p values, TCT ANOVA, p. 6:** "The main effect of training group was not significant, F(2, 90) = 2.52, p = .964" and "the interaction between time of measurement and training group was not significant, F(2, 90) = 2.52, p = .111".
- **Confidence intervals that do not contain their own means, Table 1, p. 5.** AR error count at T2: "M 7.06, SE .68, 95%-CI [2.86, 4.55]" — and this interval is identical to the VR error count T1 interval "M 3.7, SE .41, [2.86, 4.55]" in the same table. VR TCT at T2: "M 9.29, SE .52, 95%-CI [5.67, 8.45]".
- **Negative standard error, Table 2, p. 5:** AR NASA TLX "SE −1.85" (all other SE entries in Tables 1 and 2 are positive).
- **Sign of the reported mean difference vs its confidence interval, p. 8:** "the VR group made statistically significantly more errors than the AR group with a mean difference of MDiff = 2.38 errors, SE = .92, 95%-CI [−4.620, −.15]".
- **Units of the age-adjusted TCT mean differences, p. 8:** reported in seconds ("MDiff = .89 s", "MDiff = .69 s", "MDiff = −0.21 s") whereas TCT is defined and tabulated in minutes (p. 5, Table 1 note: "Task completion times (TCTs) are reported in minutes").
- **Partial eta squared magnitude vs the F and df printed alongside it, p. 8:** "there was a significant effect of training group on TCT on T1 after adjusting for age, F(2, 89) = 6.36, p < .05, η2p = .66", while the comparable age-adjusted error-count effect is reported as "F(2, 91) = 3.52, p < .05, η2p = .07".
- **Prose vs Table 4 on the age-adjusted TCT T1 comparison between AR and video, pp. 8–9:** text p. 8 gives "AR and the video group (MDiff = −0.21 s, SE = .35, 95%-CI [-.91, .49], p = .056)" and Table 4 (p. 9) records for TCT under "Objective performance when controlled for age": Video "not different from AR and VR", AR "not different from video".

---

# Paper 2 — `skill-retention-after-desktop-and-head-mounted-display-virtual-reality-training.pdf` (9 PDF pages)

## Citation as printed
- **Authors:** Alexander Farr, Leon Pietschmann, Paul Zürcher, Thomas Bohné (p. 1). Cite-this line: "Farr A, Pietschmann L, Zürcher P, Bohné T (2023)" (p. 6)
- **Year:** 2023 (pp. 1, 6)
- **Title:** "Skill retention after desktop and head-mounted-display virtual reality training" (pp. 1, 6)
- **Journal:** Experimental Results (pp. 1, 6)
- **Volume:** 4 (pp. 1, 6)
- **Issue:** not printed. Article identifier e2 (pp. 1, 6)
- **Page range:** "1–9" (pp. 1, 6)
- **DOI:** doi:10.1017/exp.2022.28 (pp. 1, 6)
- Additional as printed: "(Received 29 October 2022; Revised 05 December 2022; Accepted 08 December 2022)" (p. 1). Open Access, CC BY (p. 1). Published by Cambridge University Press (p. 1). Open peer review materials at http://doi.org/10.1017/exp.2022.28 (p. 5); two signed reviews appear on PDF pp. 7–9.

## Type of work, in the paper's own words
- Header labels on p. 1: "ENGINEERING" and "NEGATIVE-RESULT"
- "To address this gap, we designed an experiment in which learners were trained and tested for the assembly of a procedural industrial task." (p. 1)
- "To test the hypotheses outlined above, a between-subjects experiment was conducted." (p. 2)

## What was studied
- **Task:** "the assembly of a procedural industrial task" (p. 1); "an unfamiliar industrial assembly task" (p. 2); physical assembly performed at a physical workstation (p. 2; Figure 3, p. 3). "None of the participants had previously assembled the task used in the experiment." (p. 2)
- **Devices:** "While the DVR training was delivered on a standard laptop computer with an external computer mouse, the IVR training was delivered using an Oculus Quest 2 headset." (p. 2) "Both groups received the same training in the same VR environment (Figure 2)." (p. 2) "The VR environment was an advanced version of the environment used by Bohné et al. (2021). It included a familiarization phase before the main training to account for the pretraining principle (Mayer, 2017)." (p. 2)
- **Conditions compared:** desktop virtual reality (DVR) vs immersive virtual reality (IVR); "we ensured that the DVR and IVR training environments were as similar as possible, only differing in their human–computer interaction method." (p. 4)
- **Recruited:** "A total of 116 participants were recruited, of which n ¼ 44 participants successfully completed the entire experiment." (p. 2). Figure 1 (p. 3) prints the flow: "Total recruited participants n = 116" → "DVR Group n = 62" and "IVR Group n = 54" → "Total valid participants n = 116"; then "DVR Group n = 56", "IVR Group n = 50", "Excluded afterwards n = 10", "n = 106"; then "DVR Group n = 13", "IVR Group n = 31", "Excluded / no-show n = 62", "n = 44".
- **Analysed:** 44 in the retention test (p. 2; Figure 1, p. 3); "Only 44 participants participated in the retention test compared to 106 valid data points for the initial assessment." (p. 4)
- **Stated reasons for exclusion / attrition:** "For data cleaning, outliners with a z-score larger than three were excluded (Osborne & Overbay, 2004). The questionnaires included attention check items to ensure participants would not give careless responses. In case of attention check failures, the participant's data were excluded. In case data points for a participant were missing, the data were excluded." (pp. 2–3) "This discrepancy was the result of the Covid-19 pandemic in the UK at the time of the experiment as many participants had to quarantine during the retention test or were not available for the additional assessment." (p. 4)
- **Participant characteristics:** "The sample in the retention test is predominantly male as the population of students and staff in engineering (the context of our study) is also mostly male (DVR: 92.31%, IVR: 96.97%). The median age for the DVR group was 21 and 19 years for the IVR group. All participants had a normal or corrected-to-normal vision. The groups were also very similar in assembly experience with 30.77% in the DVR and 27.27% in the IVR group reporting having no assembly experience." (p. 2) "as the population in the training center was predominantly male, the same applies to our sample (3 women and 103 men in the initial test)" (p. 4)
- **Assignment and ethics:** "Participants were randomly split into two groups and trained in the execution of an unfamiliar industrial assembly task using either DVR or IVR." "The experiment received ethical approval from the University of Cambridge." (p. 2)
- **Measures:** "Metrics measured included the time to completely assemble the component (TTC) and the number of mistakes made. The latter was determined using a standardized error counting sheet. While participants could ask for a hint at any time, each hint was counted as an error." (p. 2) Affective measures: "Satisfaction, self-efficacy, and motivation were each assessed with multiple 7-point Likert questions." (Table 1 note, p. 4)

## The testing schedule
- "After the training, participants assembled the physical components in a real-world context. A researcher observed the physical assembly and recorded the performance." (p. 2)
- "The assessment procedure was repeated after 14 days (±2 days) after the initial training to determine the degree of retention for both groups (Figure 3)." (p. 2) — the "±" glyph renders as a control character in the PDF text layer; the visual page shows "14 days (2 days)" with the sign.
- Figure 1 (p. 3) prints the phase sequence and durations: "Pre-survey 3 min" → "Familiarisation 5 min" → "Training 12 min" → "Post-Training Survey 11 min" → "Assessment 16 min" → "Post-Assessment Survey 2 min", then "~14 days", then under the bracket "Retention Test": "Assessment Retention 1" → "Post-Assessment Survey 1", labelled "13 min".
- The results reported in Table 1 are for the retention test only: "Result for assessment in retention test" (Table 1 column header, p. 4). No statistics for the initial assessment are printed anywhere in the article.

## Every quantitative result
All inferential results appear in Table 1 (p. 4). Column headings: "Category", "Outcome measure", "Hypotheses", "Result for assessment in retention test", "Means of indicators: DVR / IVR".

| Category | Outcome measure | Hypothesis as printed | Result | p | DVR mean | IVR mean |
|---|---|---|---|---|---|---|
| Objective performance factors | Number of mistakes | "H1: IVR > DVR" | ✖ | "p ¼ .28" | 10.92 | 10.52 |
| Objective performance factors | TTC | "H2: IVR > DVR" | ✖ | "p ¼ .47" | 637.46 | 615.81 |
| Affective factors | Satisfaction | "H3: IVR > DVR" | ✖ | "p ¼ .42" | 5.71 | 6.14 |
| Affective factors | Self-efficacy | "H4: IVR > DVR" | ✖ | "p ¼ .55" | 5.17 | 5.21 |
| Affective factors | Motivation | "H5: IVR > DVR" | ✖ | "p ¼ .20" | 5.65 | 6.06 |

Table 1 notes (p. 4): "A cross indicates that the associated hypothesis is rejected. The number of mistakes was counted manually during the physical assembly. TTC is in seconds. Satisfaction, self-efficacy, and motivation were each assessed with multiple 7-point Likert questions."

Other printed numbers:
- "A total of 116 participants were recruited, of which n ¼ 44 participants successfully completed the entire experiment." (p. 2)
- "(DVR: 92.31%, IVR: 96.97%)" male; "The median age for the DVR group was 21 and 19 years for the IVR group"; "30.77% in the DVR and 27.27% in the IVR group reporting having no assembly experience" (p. 2)
- "A 5% significance level was used" (p. 3)
- Figure 1 group and flow numbers, and phase durations, as listed above (p. 3)
- "Only 44 participants participated in the retention test compared to 106 valid data points for the initial assessment." "3 women and 103 men in the initial test" (p. 4)

**Test names, statistics, degrees of freedom, standard deviations:** the tests actually used per outcome are not identified, and no test statistics, degrees of freedom, effect sizes, or standard deviations are printed. The analysis plan states only: "Homoscedasticity of data was assessed with Shapiro–Wilk tests and Levene tests were used to assess the normality of the data. If the data were homoscedastic and normally distributed for all groups, ANOVAs were used to compare the data and Wilcoxon rank-sum-tests otherwise." (p. 3) Which of ANOVA or Wilcoxon produced each of the five p values in Table 1 is not stated.

## Direction of each non-significant difference
All five tests are non-significant (p. 4: "All tests are nonsignificant."). Directions from the means in Table 1 (p. 4):
- **Number of mistakes (p = .28):** IVR numerically fewer (IVR 10.52 vs DVR 10.92).
- **TTC (p = .47):** IVR numerically lower/faster (IVR 615.81 s vs DVR 637.46 s).
- **Satisfaction (p = .42):** IVR numerically higher (IVR 6.14 vs DVR 5.71).
- **Self-efficacy (p = .55):** IVR numerically higher (IVR 5.21 vs DVR 5.17).
- **Motivation (p = .20):** IVR numerically higher (IVR 6.06 vs DVR 5.65).
- The authors state the same on p. 4: "All descriptive indicators indicate a better performance of the IVR group as the number of mistakes and TTC are lower and the affective factors have been rated higher by the IVR group."

## Authors' own stated limitations and future work
- p. 4: "A limitation of our study is the relatively small sample size in the retention test. Only 44 participants participated in the retention test compared to 106 valid data points for the initial assessment. This discrepancy was the result of the Covid-19 pandemic in the UK at the time of the experiment as many participants had to quarantine during the retention test or were not available for the additional assessment."
- p. 4: "Additionally, as the population in the training center was predominantly male, the same applies to our sample (3 women and 103 men in the initial test). Results might be different for a more balanced sample."
- p. 4: "Three further areas of research are proposed. First, the future experiment should replicate according to the original plan with several retention tests and sufficient participants in the retention tests, as this has still been rarely analyzed in the literature."
- pp. 4–5: "Second, the transferability of the results should be investigated by running the experiment with a different training content to test if the findings hold outside an industrial training context."
- p. 5: "Third, retention of VR training should be compared with retention after learning with other media such as paper or videos."
- p. 5: "Because of the Covid pandemic, the sample in our study was smaller than planned which might explain the non-significant results."
- p. 5: "Decisions for a media (DVR or IVR) should consider the results of this study and other factors that were not examined in our study including cost, software development complexity, the appropriateness of a task for DVR or IVR, and the required infrastructure for DVR or IVR training."

## Five verbatim headline claims
1. "We found nonsignificant differences in the number of errors, the time to completion, satisfaction, self-efficacy, and motivation." (p. 1)
2. "The results support the view that DVR and IVR are similarly useful for learning retention." (p. 1)
3. "All descriptive indicators indicate a better performance of the IVR group as the number of mistakes and TTC are lower and the affective factors have been rated higher by the IVR group." (p. 4)
4. "The results of our experiment suggest that there are nonsignificant differences in performance in retention after learning with DVR or IVR and that these technologies could be used interchangeably." (p. 5)
5. "Because of the Covid pandemic, the sample in our study was smaller than planned which might explain the non-significant results." (p. 5)

## Internal inconsistencies
- **Abstract vs participants section on sample size:** the abstract (p. 1) prints no sample size — "we designed an experiment in which learners were trained and tested for the assembly of a procedural industrial task" — so on this specific point, none found.
- **Retention-test group sizes implied by the reported percentages vs Figure 1.** Text, p. 2: "(DVR: 92.31%, IVR: 96.97%)" male, and "30.77% in the DVR and 27.27% in the IVR group reporting having no assembly experience". 92.31% and 30.77% correspond exactly to 12/13 and 4/13 (DVR n = 13, matching Figure 1). 96.97% and 27.27% correspond exactly to 32/33 and 9/33, i.e. an IVR n of 33, whereas Figure 1 (p. 3) prints "IVR Group n = 31" and the total "n = 44" (13 + 31 = 44).
- **Recruitment vs valid-participant labels in Figure 1 (p. 3):** "Total recruited participants n = 116" and, after the training/assessment column, "Total valid participants n = 116", while the next column prints "Excluded afterwards n = 10" against "n = 106".
- **Hypothesis direction notation, text vs Table 1.** Text, p. 1: "H1. The number of errors in the physical assembly in the retention test is lower for participants trained with IVR than those trained with DVR." Text, p. 2: "H2. Time to completion (TTC) of the physical assembly in the retention test is lower for participants trained with IVR than those trained with DVR." Table 1, p. 4, prints both as "H1: IVR > DVR" and "H2: IVR > DVR", the same notation used for the three affective hypotheses where higher scores are predicted.
- **Test-purpose statement in the analysis plan, p. 3:** "Homoscedasticity of data was assessed with Shapiro–Wilk tests and Levene tests were used to assess the normality of the data."
- **Rejection wording vs the stated hypotheses, Table 1 note (p. 4) and Discussion (p. 4):** the table marks all five hypotheses "✖" with the note "A cross indicates that the associated hypothesis is rejected", while p. 4 states "All tests are nonsignificant."

---

# Paper 3 — `Immersive_Mixed_Reality_for_Manufacturing_Training.pdf` (8 PDF pages)

## Citation as printed
- **Authors:** Mar Gonzalez-Franco, Rodrigo Pizarro, Julio Cermeron, Katie Li, Jacob Thorn, Windo Hutabarat, Ashutosh Tiwari, Pablo Bermell-Garcia (p. 1). Citation block: "Gonzalez-Franco M, Pizarro R, Cermeron J, Li K, Thorn J, Hutabarat W, Tiwari A and Bermell-Garcia P (2017)" (p. 1)
- **Year:** 2017 (p. 1)
- **Title:** "Immersive Mixed Reality for Manufacturing Training" (p. 1)
- **Journal:** Frontiers in Robotics and AI (p. 1); citation form "Front. Robot. AI" (p. 1)
- **Volume:** 4 (p. 1)
- **Issue:** not printed. Article number 3 ("Volume 4 | Article 3", p. 1; "Front. Robot. AI 4:3", p. 1)
- **Page range:** not printed. PDF footers number the article 1–8.
- **DOI:** doi: 10.3389/frobt.2017.00003 (p. 1)
- Additional as printed: "Original Research / published: 16 February 2017"; "Received: 08 July 2016; Accepted: 27 January 2017; Published: 16 February 2017"; Edited by John Quarles; Reviewed by Ana Tajadura-Jiménez and Regis Kopper; Specialty section "Virtual Environments" (p. 1). CC BY (p. 8).

## Type of work, in the paper's own words
- Label on p. 1: "Original Research"
- "In this paper, we present an experiment (n = 20) that shows how modern metaphors such as collaborative mixed reality can be used to transmit procedural knowledge and could eventually replace other forms of face-to-face training." (p. 1)
- "In this paper, we validate whether a MR setup could work for complex manufacturing training and we compare the results to conventional face-to-face training done on a physical scaled model." (p. 2)
- "Participants were randomly assigned to one of the two experimental conditions in a between subjects' study" (p. 4)
- "Twenty-four volunteers ... participated in the user study." (p. 3)

## What was studied
- **Task:** "We reproduced an aircraft maintenance door training manual in our MR setup." (p. 3) "In both conditions, participants underwent the same procedural script obtained from a complex manufacturing manual of an aircraft maintenance door." (p. 4) "The trainer performed the inspection and operated the moving parts of a door assembly jig following the manual. During this phase, the trainee had to observe what the trainer was doing and tried to remember as much as possible for the evaluation phase." (p. 4)
- **Device:** "We built a mixed reality setup by modifying an Oculus Rift DK1 HMD with a 1,280 × 800 resolution (640 × 800 per eye), a 110° diagonal field of view (FOV) and approximately 90° horizontal FOV. A pair of cameras were mounted to the HMD to form a see-through mixed reality setup" (p. 2); "The scenario was implemented in Unity 3D, and the head tracking was performed with a NaturalPoint Motive motion capture system (24× Flex 13 cameras) running at 120 Hz" (p. 2); interaction via "a rigid body reflective marker to an Ipow z07-5 stick" (p. 3); multi-user collaboration "through a PhotonServer installed in the laboratory" (p. 3); frame rates "(~45 and 60 Hz, respectively)" (p. 3). "For the conventional face-to-face training condition, we manufactured a laser-cut physical model of the jig in transparent plastic" (p. 3).
- **Conditions compared:** "(i) conventional face-to-face training, where participants were taught in a traditional face-to-face scenario manipulating a scaled assembly jig; and (ii) MR training. In the MR, participants were taught in a face-to-face scenario with a see-through HMD." (p. 4) Figure 2 (p. 3) prints "Condition 1 - Conventional face-to-face training (x12 Participants)" and "Condition 2 - Immersive MR training (x12 Participants)".
- **Recruited / analysed:** "Twenty-four volunteers (age mean = 32.5, SD = 9.6 years, three females) participated in the user study." (p. 3). The abstract states "an experiment (n = 20)" (p. 1). No exclusions, dropouts, or attrition are reported anywhere in the article; no per-condition analysed N is printed in the Results section. Figure 2 (p. 3) prints 12 per condition.
- **Recruitment and eligibility:** "Due to the confidential nature of the manufacturing content, this study was conducted using only employees from the institution. Participants who volunteered for the study did not have previous manufacturing knowledge and were asked to complete a demographic questionnaire before participating." (p. 3) "Due to the confidential nature of the manufacturing content, this study was conducted using only employees from the Airbus Group who volunteered to participate. They were recruited via email." (p. 6)
- **Ethics:** "Following the Declaration of Helsinki all participants gave informed consent. This study was approved by the Science and Engineering Research Ethics Committee (SEREC) of Cranfield University." (p. 3, repeated p. 6)
- **Measures:** "the trainee was asked to complete two tests (a knowledge retention and a knowledge interpretation test)" (p. 4). "The knowledge retention test was a written test using a multiple-choice format with eight questions (Table 1). This test was designed to evaluate how much factual knowledge was retained from the training" (p. 4); the eight multiple-choice items are printed in full in Table 1 (p. 4). "The knowledge interpretation test evaluated whether the whole procedure of the assembly was properly captured. This test was executed in a scaled physical jig and the trainee was asked to perform step by step significant parts of the assembly training until completing the whole operation. If at any point the participant skipped a step or required intervention from the experimenter (e.g., one of the drills was not performed), this reduced one point in the score. The maximum score was 43, the equivalent to the sum of actions that were required to complete the operation." (p. 4) Training duration was also recorded (p. 5).

## The testing schedule
- "After the training, the trainee was asked to complete two tests (a knowledge retention and a knowledge interpretation test) to compare both types of trainings." (p. 4)
- "Participants were then evaluated to assess how much knowledge they captured during the training (Figure 2)." (p. 4)
- Figure 2 (p. 3) shows a single linear sequence: "Training" (Condition 1 / Condition 2) → "Evaluation" comprising "Knowledge Retention Test" then "Knowledge Interpretation Test". No time gap, delay, or second session is depicted or labelled.
- No interval in hours, days, or weeks between training and testing is stated anywhere in the article; the word "retention" is used for the immediate written multiple-choice test. The number of training sessions is stated as one: "with a single training exposure, participants could complete the task and tests" (p. 4) and "with a single training session in a manufacturing operation that was totally novel to them" (p. 5).

## Every quantitative result
- **Knowledge retention (p. 4):** "No significant differences were found for knowledge retention (scores from 0 to 8) between the two conditions [Kruskal–Wallis rank sum test χ2(1) = 0.1, p = 0.7]. The score for the MR condition was (M = 3.75, SD = 1.21), and the score for the conventional condition was (M = 3.91, SD = 1.44)."
- **Knowledge interpretation (p. 4):** "No significant differences were found for knowledge interpretation (scores from 0 to 43) between the two conditions [Kruskal–Wallis rank sum test χ2(1) = 1.9, p = 0.16]. The score for the MR condition was (M = 35.41, SD = 8.03), and the score for the conventional face-to-face condition was (M = 39.25, SD = 4.86)."
- **Equivalence testing (pp. 4–5):** "We ran an additional Two One-Sided Test (TOST) for equivalence and found that for the knowledge retention both populations showed a confidence level over 93%, indicating a trend in equivalence for the retention between the MR and the conventional face-to-face conditions." (p. 4) "The same test on the knowledge interpretation did not show such a high equivalence and was rejected (p = 0.84); therefore, the knowledge interpretation results were not conclusive since although they were not significantly different they were also not significantly equivalent." (p. 5)
- **Correlation between the two tests (p. 5):** "while in the MR condition a correlation trend was found between high scores in the interpretation and retention [Pearson r(12) = 0.57, p = 0.052], this was not true for the conventional face-to-face training condition (p > 0.39)"
- **Training time (p. 5):** "The time spent to complete the training was significantly higher in the MR condition (M = 12.1, SD = 2.5 min) than in the conventional face-to-face training condition (M = 9.9, SD = 0.9 min) [Kruskal–Wallis rank sum test χ2(1) = 0.64, p = 0.01]"
- **Score level statement (p. 5):** "Very high scores were found in the interpretation test in both conditions, scoring over 80% of accuracy with a single training session"
- **Sample and demographics:** "an experiment (n = 20)" (p. 1); "Twenty-four volunteers (age mean = 32.5, SD = 9.6 years, three females)" (p. 3); "(x12 Participants)" per condition (Figure 2, p. 3)
- **Apparatus figures:** resolution 1,280 × 800 (640 × 800 per eye), 110° diagonal FOV, ~90° horizontal FOV, 24× Flex 13 cameras at 120 Hz, camera/HMD frame rates ~45 and 60 Hz, maximum interpretation score 43, eight retention questions (pp. 2–4)
- No effect sizes and no confidence intervals are printed for any comparison; the TOST is reported as a "confidence level over 93%" for retention and "p = 0.84" for interpretation.

## Direction of each non-significant difference
- **Knowledge retention (χ2(1) = 0.1, p = 0.7, p. 4):** the conventional face-to-face condition scored numerically higher (conventional M = 3.91, SD = 1.44 vs MR M = 3.75, SD = 1.21) (p. 4).
- **Knowledge interpretation (χ2(1) = 1.9, p = 0.16, p. 4):** the conventional face-to-face condition scored numerically higher (conventional M = 39.25, SD = 4.86 vs MR M = 35.41, SD = 8.03) (p. 4).
- **Correlation in the MR condition (r(12) = 0.57, p = 0.052, p. 5):** positive direction — "participants who were better in the interpretation task were also better in the retention task" (p. 6).
- **Correlation in the conventional condition (p > 0.39, p. 5):** no r value and no direction are printed.
- **TOST for knowledge interpretation (p = 0.84, p. 5):** direction not applicable as printed; the authors state the equivalence test "was rejected" and that the results "were not significantly different they were also not significantly equivalent" (p. 5).
- **Descriptive statement on subgroups (p. 5):** "it seems that top performing participants in the MR condition were as good as the ones in the conventional training. However, low performing participants in the MR were worse." No statistic accompanies this statement.

## Authors' own stated limitations and future work
No section is headed "Limitations". Statements as printed:
- p. 3: "Due to the confidential nature of the manufacturing content, this study was conducted using only employees from the institution."
- p. 5: "We hypothesize that low performers may have been overwhelmed by the setup and that constrained their capacity to capture knowledge; however, this effect may fade away as participants become more used to the technology itself." (statement appears on PDF p. 4, continuing text; the sentence is printed at the foot of the Results column on p. 4)
- p. 5: "the knowledge interpretation results were not conclusive since although they were not significantly different they were also not significantly equivalent."
- p. 5: "However, equivalence results failed to show significance between participants in the MR and the conventional face-to-face conditions."
- p. 5: "In general, the scores in the retention test were low, we hypothesize that there might be two reasons to the difference in performance between the retention and the interpretation knowledge. First, the complexity of the task might require several training sessions to be properly retained. Second, we believe that, given the type of training, the participants developed a more hands-on memory of the procedure than an abstract knowledge."
- p. 6: "These results are aligned with previous studies that show higher cognitive load is needed when using novel technologies at first (Chen et al., 2007), and the MR setup might have placed some participants outside their comfort zone, making them unable to remember or guess what to do next. This would also contribute toward explaining the results that show that participants took longer in the MR condition than in the conventional face-to-face condition, because they were less familiar with the environment."
- p. 6: "However, this setup would not be a complete substitute of a face-to-face training, since there will still be a need of professional trainers. Therefore, only one part of the overhead training costs would be reduced."
- p. 1: "These results and their implications for future training and the use of virtual reality, mixed reality, and augmented reality paradigms in this context are discussed in this paper."

## Five verbatim headline claims
1. "The obtained results indicate that performance levels in the immersive mixed reality training were not significantly different than in the conventional face-to-face training condition." (p. 1)
2. "In this paper, we present an experiment (n = 20) that shows how modern metaphors such as collaborative mixed reality can be used to transmit procedural knowledge and could eventually replace other forms of face-to-face training." (p. 1)
3. "Overall, we found that the knowledge levels acquired both in the mixed reality setup and in the conventional face-to-face setup were not significantly different." (p. 5)
4. "The results show trends of equivalent knowledge retention between MR training and the conventional face-to-face training." (p. 6)
5. "However, no significant differences or significant equivalences were found between the two conditions for knowledge interpretation." (p. 6)

## Internal inconsistencies
- **Abstract vs participants section on sample size.** Abstract, p. 1: "In this paper, we present an experiment (n = 20) that shows how modern metaphors such as collaborative mixed reality can be used to transmit procedural knowledge". Participants section, p. 3: "Twenty-four volunteers (age mean = 32.5, SD = 9.6 years, three females) participated in the user study." Figure 2, p. 3, prints "Condition 1 - Conventional face-to-face training (x12 Participants)" and "Condition 2 - Immersive MR training (x12 Participants)" (12 + 12 = 24). No exclusion, dropout, or data-loss step is reported anywhere that would reconcile 24 with 20.
- **Degrees of freedom of the MR correlation vs the condition size printed in Figure 2.** Results, p. 5: "[Pearson r(12) = 0.57, p = 0.052]" (df = 12 corresponds to n = 14 pairs) against Figure 2, p. 3: "Condition 2 - Immersive MR training (x12 Participants)".
- **Test statistic vs p value for the training-time comparison, p. 5:** "The time spent to complete the training was significantly higher in the MR condition (M = 12.1, SD = 2.5 min) than in the conventional face-to-face training condition (M = 9.9, SD = 0.9 min) [Kruskal–Wallis rank sum test χ2(1) = 0.64, p = 0.01]". The same article reports a larger statistic with a much larger p value for another comparison on p. 4: "χ2(1) = 1.9, p = 0.16", and "χ2(1) = 0.1, p = 0.7".
- **"Retention" terminology vs the schedule printed.** The measure is named "knowledge retention test" and "how much factual knowledge was retained from the training" (p. 4), while the procedure states "After the training, the trainee was asked to complete two tests" (p. 4) and Figure 2 (p. 3) shows Evaluation immediately following Training with no interval.
- **Institution named for the participant pool.** p. 3: "this study was conducted using only employees from the institution"; p. 6: "this study was conducted using only employees from the Airbus Group who volunteered to participate."
- **Correlation described as a "trend" at the 5% level, p. 5:** "a correlation trend was found between high scores in the interpretation and retention [Pearson r(12) = 0.57, p = 0.052]", while p. 6 states "We did, however, find a correlation between high interpretation and retention scores in participants who completed the training through MR".agentId: a49782370f1b9c907 (use SendMessage with to: 'a49782370f1b9c907', summary: '<5-10 word recap>' to continue this agent)
<usage>subagent_tokens: 130237
tool_uses: 15
duration_ms: 344859</usage>
---

# PART 4 — Danielsson, Sik Lanyi & Withers, Koutromanos & Kazakou

I have complete extractions for all three papers. Writing the evidence profile.

---

# Evidence profile: three PDFs

Note on quoting: all quotations are transcribed from the pymupdf text layer with column line-breaks and the PDF's soft hyphens removed (e.g. `spec­ifications` → `specifications`); no words were changed, added or reordered. All page numbers are pymupdf 1-indexed PDF pages.

---

## Paper 1 — `AR smart glasses in industrial assembly, status and challenges.pdf` (10 PDF pages)

### Citation as printed
- **Authors:** Oscar Danielsson, Magnus Holm, Anna Syberfeldt (p. 1). Affiliation printed: "Production and Automation Engineering, University of Skövde, Kanikegränd 3A, Skövde, Sweden" (p. 1).
- **Year:** 2020 (p. 1, running header and copyright line "2452-414X/© 2020 Elsevier Inc.").
- **Full title:** "Augmented reality smart glasses in industrial assembly: Current status and future challenges" (p. 1).
- **Journal:** Journal of Industrial Information Integration (running header on every page; also masthead on p. 1).
- **Volume:** 20 (running header "Journal of Industrial Information Integration 20 (2020) 100175", pp. 1–10).
- **Issue:** not printed.
- **Page range:** not printed. An article number is printed instead: 100175 (running header, pp. 1–10).
- **DOI:** https://doi.org/10.1016/j.jii.2020.100175 (p. 1).
- Other printed dates: "Available online 13 October 2020" (p. 1). Received/accepted dates: not printed.

### Type of work, in the paper's own words
- "A literature survey considers both these perspectives of ARSG." (p. 1)
- Keyword listed: "Literature survey" (p. 1).
- "This literature survey had the aim to identify what still needs to be solved before ARSG implementation on the shop floor is possible by surveying the current status of ARSG from an industrial perspective." (p. 8)
- Section 3 is titled "Structure of the literature survey" (p. 3).
- The authors describe a *prior* paper of theirs as the scoping review: "Fig. 1 presents the results of a scoping review of ARSG for industrial assembly operators [9]." (p. 3)

### Search and selection method
**Not stated.** No databases, search strings, date window, screening stages or record counts appear anywhere in pp. 1–8. Section 3 ("Structure of the literature survey", p. 3) defines only the two perspectives and their sub-topics, taken from reference [9]: manufacturing engineering (authoring, infrastructure, validation) and technological maturity (ARSG, enabling technology, technological demands, tracking).

### Every quantitative or categorical finding (with pages)

Readiness levels:
- "The Technological Readiness Level (TRL) of some of the components of ARSG is still low, with displays having a TRL of 7 and tracking a TRL of 5." (p. 1)
- "While smart glasses (SG) in themselves have been found to have a TRL of 9, AR technologies are at a lower level [43]." (p. 4)
- "For industrial use, 'Augmented Reality Tracking Techniques', 'Interaction Techniques and User Interfaces', and 'Augmented Reality SDK's' all have a TRL of 5 and 'Augmented Reality Display Technologies' have a TRL of 7 according to [43]." (p. 4)
- "It was similarly found by [44] that AR has a TRL between 6 and 7." (p. 4)
- "The TRL of AR has been found to be between 5 and 6 for military use [45]." (p. 4)
- "In medicine AR TRL follows a rough bell curve from 4 to 7 [46]." (p. 4)
- Table 4 restates: "SG TRL 9", "AR displays TRL 7", "Tracking, interaction, and UI TRL 5" (p. 8).
- Answer A 2: "While the general TRL of SG is 9, the individual parts needed to improve SG into ARSG are still at a lower TRL. AR displays are at TRL 7 and tracking, interaction, and UI at TRL 5" (p. 8).

Device market data (Table 1, "Non exhaustive list of ARSG (extended from [6, 53, 54]", spans pp. 5–6). 18 entries, columns Company / Name / Release / Weight (g) / FOV (diagonal) / Battery hours / Battery mAh:
- 2013: Penny C Wear Extended 115 g, 47°, –, –; VUZIX M100 72 g, 15°, 1–6 h, 3800 mAh
- 2014: Epson Moverio BT-200 96 g, 23°, 6 h, 2720; Optinvent ORA-2 90 g, 23°, 3 h
- 2015: Atheer AiR Glass 350 g, 50°, 8 h, 3100; ODG R-7 170 g, 30°, 1–6 h, 1300; Sime G3 72 g, 20°, 1 h; Sony SmartEyeglass 77 g, 20°, 1.3 h, –
- 2016: DAQRI Smart glasses 335 g, 44°, –, 5800; Epson Moverio BT-300 69 g, 23°, 6 h; Epson Moverio BT-2000 290 g, 23°, 4 h, 2 × 1240; Shadowcreator Halo Mini 370 g, 40°, –, 3200; Microsoft Hololens 579 g, 35°, 5 h, 16,500
- 2017: VUZIX M300 140 g, 20°, 2–12 h, "160 internal 860 external"; Metavision Meta 2 420 g, 90°, Tethered, Tethered
- 2018: Magic Leap One creator edition "325 head + 415" g, 50°, 3 h, –
- "pre-order": Microsoft Hololens 2 566 g, 52°, –, –
- no release year: Shadowcreator Action One –, 45°, –, 4000
- Italic text in Table 1 marks discontinued/exited/bankrupt companies: "This is indicated in Table 1 with italic text." (p. 5)

Aggregate statements about the device data:
- "Findings include that commercially available ARSG differ considerably in their hardware specifications." (p. 1)
- "Weight varies between 69 and 579 g. The FOV ranges from 15 to 90° diagonally and the battery capacity varies between 1 and 12 h." (p. 5)
- "The latest generation of ARSG, for example Microsoft Hololens 2 and Magic Leap One creator edition has achieved a FOV of 52 and 50° respectively. For the Hololens 2 this is an improvement of about 50% in three years." (p. 5)
- Table 4: "FOV of 52 degrees diagonally commercially available (100 experimentally)" (p. 8).

Field of view and human vision:
- "A person that has a normal eye vision has a FOV of about 150° with one eye and about 180° with two eyes [50]." (p. 5)
- "An experimental setup makes it possible to achieve a FOV of 100° diagonally [52]." (p. 5)

Connectivity/performance figures cited:
- "AR has been identified to need a high data rate of around 25 Mbps as well as a very low latency of around 1 ms [32]." (p. 3)
- "The IR markers prototyped by Urtans and Nikitenko [63] worked up to 1.5 m from the camera with comparable fiducial markers working up to 3 m away. Similarly they found that marker identification time was 290 ms, three times slower than with IR markers" (p. 6)
- RFID with depth cameras: "managed to identify and distinguish up to 15 tags simultaneously with an average match ratio of 91%." (p. 6)
- UWB "detect the user position through sending a signal from the user and then using triangulation of the signal to 3–4 base stations around the user [68]." (p. 6)
- Magnetic beacons: "requires the installation of magnetic beacons at intervals of 4–6 m" (p. 7)
- Wi-Fi: "the accuracy is usually only within several meters... Wi-Fi is also limited to 3 degrees of freedom." (p. 6)
- Tracking-technique shares in cited reviews: "90% of the articles of AR studied in maintenance used this technique, with marker-based tracking being the most common (52%)." (p. 7)

Classifications the authors produce or reproduce:
- Two perspectives with sub-topics (Fig. 1, p. 2; defined p. 3): manufacturing engineering = authoring, infrastructure, validation; technological maturity = ARSG, enabling technology, technological demands, tracking.
- Three display-placement classes: "on the user's head (head-mounted), in the user's hand (handheld), or installed in the environment (spatial) [12,13]." (p. 2)
- Five HMD AR implementations: projection based, eye multiplexed, optical see-through, video see-through (p. 2), plus "A fifth way of implementing AR in HMD's is retinal projection" (p. 2).
- Three display choices: "monocular (one eye views a screen), binocular (both eyes view the same screen), or dichoptic (each eye views a different screen, enabling depth perception) [20]." (p. 3)
- Three AR requirements: "(1) Combine reality and virtuality (2) Real time interactivity (3) Registered in 3D" (p. 6).
- Three industrial AR requirement categories (from [8]): development/integration (cost-effectiveness, data security, applicable regulations), set-up (set-up time, system reliability), operational (accuracy of presentation, real-time capability, ergonomics) (p. 3).
- Table 2, "List of tracking technologies, adapted from [56–58]" — 15 technologies scored for Environment and Industrial suitability (p. 6): Electro-magnetic: Bluetooth (in, Yes), Differential GPS (out, No), GPS (out, No), Infrared (in, Yes), Real-time kinematic (RTK) GPS (out, No), RFID (in, Yes), UWB (in, No), Wi-Fi (in, Yes); Inertial: Accelerometer (in/out, Yes), Gyroscopes (in/out, Yes); Magnetic: Magnetometers (in/out, Limited), Magnetic beacons (in, Unknown); Ultrasonic: Ultrasonic (in, No); Vision: Markers (in/out, Yes), Nature feature (in/out, Yes).
- Vision-based tracking taxonomies reproduced as figures: Fig. 2 "Vision based tracking categorization made by [86]" — "defines two main categories of methods: 3D model based techniques and Coplanar based techniques" (caption p. 5; text p. 7); Fig. 3 "Vision based tracking categorization made by [5]" (p. 7).
- Four AR-tracking categories from [88]: "Model-based, Features-based, Marker-based, and Others" (p. 7).
- Areas needing improvement from [6]: "extending the field of view (FOV), making the glasses wearable, developing guidelines for user interface design, enabling benchmark evaluation, and improving voice-based interaction in noisy environments." (p. 4)
- Table 3, "Summary of findings in the manufacturing engineering perspective" — current status / future challenges per topic (p. 7).
- Table 4, "Summary of findings in technological maturity perspective" — current status / future challenges per topic, including "Emerging market", "Few strong actors taking lead", "Improve battery life", "Reduce price", "Reduce weight", "Increase FOV", "Bluetooth and RFID suitable and synergy is possible", "IMU MEMS sensors can improve visual tracking", "MEMS sensors accuracy is lower", "Reduce magnetometer sensitivity to noise" (p. 8).
- Authoring result cited: authored AR content "led to participants performing the maintenance tasks at around half the time of that of those using the paper-based instructions [22]." (p. 3)
- Country concentration cited from [11]: "more than half of all journal papers with an AR focus in manufacturing industry featured only five countries: Singapore, Germany, Italy, USA, and China" (p. 2).

### Hypothesis stated, and where confirmed or rejected
**No hypothesis is stated.** The paper poses three questions instead:
- "Three questions were identified as important to seek answers for in this survey" — Q 1, Q 2, Q 3 (p. 2).
- The answers are given as "A 1", "A 2", "A 3" (p. 8).

The nearest thing to a posited premise, stated as an assumption rather than a hypothesis: "This is motivated by the assumption that processing power and batteries will improve over time to a point where all the necessary performance for most uses can fit in a device that can be worn like regular glasses, at which point there would be no added value in increasing the size of a pair of ARSG." (p. 2). No verdict on this assumption is stated anywhere in the paper.

### What the authors say is missing from the field
- "In general there is a lack of standards for AR regarding vertical industry application in industrial scenarios according to [38]. They more specifically identify a lack of human-machine interaction standards and unified norms, high construction cost, and a lack of references for enterprises to the deployment of AR." (p. 4)
- "And there are as yet no long term studies regarding the use of AR hardware, which relates to Section 4.3 [40]." (p. 4)
- "A current limitation is that: 'Published evaluation and test results often cover out-of-date hardware or prototype systems.' [35, p.26]." (p. 4)
- "Guidelines for making strategic decisions regarding ARSG for assembly exists but are still in need of validation and do not take economic aspects into account." (p. 4)
- "In summary, testing ARSG in a systematic and comparative way is still an emerging topic, where there are needs to further extend aspects that can be evaluated to improve the adaptability of test-designs." (p. 4)
- "The technologies for automatic content creation are however still not robust enough to reliably create instructions in the general case and need to be further improved." (p. 8)
- "Maintenance of ARSG software as a topic is less explored, possibly due to the early stages of adaptation in the manufacturing industry." (p. 8)
- "There is a need to further study the manufacturing engineering perspective to identify relevant parameters for ARSG as assembly operator support and how this can be practically achieved and maintained over time." (p. 8)
- "Current battery-technology or techniques to minimize battery drain do not allow full usage of ARSG through a normal workday and even experimental setups cannot reach the FOV of the human eye." (p. 5)
- "There is currently no video-based or optical see-through HMD that can provide AR with the same FOV as that of a human." (p. 5)

### The authors' own stated limitations and future work
- Scope limitation: "Manufacturing engineering is a very broad area and within this paper the scope is limited to the aspects of integrating, maintaining and updating ARSG as a support tool for operators in assembly lines." (p. 2)
- "To implement a pair of ARSG there are a multitude of technologies needed and it is beyond the scope of this survey to present an exhaustive analysis of them all." (p. 4)
- Excluded by scope: GPS ("Since GPS-based solutions only work outdoors [56,62], they are not further considered in this survey.", p. 6); ultrasonic ("It is therefore not explored further in this survey.", p. 7).
- Table 1 is declared non-exhaustive: "Non exhaustive list of ARSG" (p. 5).
- "This survey has focused on the technical and manufacturing engineering perspective of ARSG as support for operators in the industrial shop floor. The operators' perspective with aspects such as ergonomics is an important perspective that is connected to the two perspectives explored in this survey, as described in [9] but have been left out due to the scope of this particular survey. A survey or literature review exploring that perspective is a suggested future work." (p. 8)
- "Future work identified is to also explore the operator perspective of ARSG in manufacturing." (p. 1)
- "Given that AR in general and ARSG specifically is just on the verge of being broadly implemented in industrial shop floors [29], it is possible that more technological demands will become apparent in the future that have not been possible to predict before ARSG becomes more widespread in manufacturing industry." (p. 4)
- Future-challenge lists per topic are in Table 3 (p. 7) and Table 4 (p. 8).
- Authors' own forward-looking statements: "Some of the future challenges the authors see is improving battery life, reducing prices, and ergonomic strain." (p. 5); "Future challenges include improving automated instruction generation to further reduce lead times for assembly instruction generation." (p. 3)

### Five verbatim sentences (headline claims)
1. "Findings include that commercially available ARSG differ considerably in their hardware specifications." (p. 1)
2. "The Technological Readiness Level (TRL) of some of the components of ARSG is still low, with displays having a TRL of 7 and tracking a TRL of 5." (p. 1)
3. "A mapping of tracking technologies and their suitability for industrial ARSG was done and identified Bluetooth, micro-electro mechanical sensors (MEMS) and infrared sensors as potentially suitable technologies to improve tracking." (p. 1)
4. "In general terms the development level of ARSG is currently at such a level that it can be used in actual production in a limited capacity but is still at an early stage of implementation and as such still needs further development before large scale adoption can readily take place." (p. 8)
5. "The technological maturity perspective can thus be summarized that while ARSG are currently not suitable for being used through an entire workday for operators, they are being improved on at a rapid pace, driven by a growing commercial market." (p. 8)

### Internal inconsistencies
1. **Which tracking technologies are named as suitable.** Abstract (p. 1): "identified Bluetooth, micro-electro mechanical sensors (MEMS) and infrared sensors as potentially suitable technologies to improve tracking." Section 5.4 summary (p. 7): "Bluetooth and RFID are two technologies that can give a good estimated position of objects and they also have further uses besides AR tracking for assembly instructions". Table 4 (p. 8): "Bluetooth and RFID suitable and synergy is possible" / "Implementing Bluetooth and RFID infrastructure". Infrared is marked "Yes" for industrial suitability in Table 2 (p. 6) but is absent from the section-5.4 summary and Table 4; RFID is absent from the abstract.
2. **Commercially available FOV.** Table 1 lists Metavision Meta 2, release 2017, "90" degrees diagonal (pp. 5–6), and the text states "The FOV ranges from 15 to 90° diagonally" (p. 5). Table 4 states "FOV of 52 degrees diagonally commercially available (100 experimentally)" (p. 8), and the text states "The latest generation of ARSG, for example Microsoft Hololens 2 and Magic Leap One creator edition has achieved a FOV of 52 and 50° respectively." (p. 5)
3. **Figure 2 is cited for two different contents.** The caption reads "Fig. 2. Vision based tracking categorization made by [86]." (p. 5), and p. 7 uses it that way: "The categorization by [86], summarized in Fig. 2, defines two main categories of methods". But p. 4 cites it as a device list — "there are many ARSG available for consumers (see Table 1 and Fig. 2)" — and p. 6 cites it as a weight/FOV relationship: "The current tradeoff that can be seen in Fig. 2 is that in general the higher the FOV the heavier the weight".
4. **Weight range vs Table 1 entry.** Text: "Weight varies between 69 and 579 g." (p. 5). Table 1 lists Magic Leap One creator edition as "325 head + 415" g (p. 6).
5. **Name of the TRL acronym.** Abstract: "The Technological Readiness Level (TRL)" (p. 1). Abbreviation list on the same page: "TRL, Technology Readiness Level" (p. 1).

---

## Paper 2 — `Acceptability and human factors in adopting AR.pdf` (19 PDF pages)

### Citation as printed
- **Authors:** Cecilia Sik Lanyi, John Dermott Alexander Withers (p. 1). Affiliation: "Department of Electrical Engineering and Information Systems, University of Pannonia, 8200 Veszprem, Hungary" (p. 1).
- **Year:** 2020 (p. 1).
- **Full title:** "Striving for a Safer and More Ergonomic Workplace: Acceptability and Human Factors Related to the Adoption of AR/VR Glasses in Industry 4.0" (p. 1).
- **Journal:** Smart Cities (masthead p. 1; running header "Smart Cities 2020, 3" pp. 2–19; "www.mdpi.com/journal/smartcities" p. 1).
- **Volume:** 3 (p. 1 footer line "Smart Cities 2020, 3, 289–307").
- **Issue:** not printed. (The DOI string printed on p. 1 reads "doi:10.3390/smartcities3020016".)
- **Page range:** 289–307 (p. 1 footer). Running page numbers 290–307 appear on pp. 2–19.
- **DOI:** doi:10.3390/smartcities3020016 (p. 1).
- Other printed dates: "Received: 15 March 2020; Accepted: 28 April 2020; Published: 2 May 2020" (p. 1). Article type printed above the title: "Review" (p. 1). Correspondence: "lanyi@almos.uni-pannon.hu" (p. 1). Funding: "grant number: 2018-1.3.1-VKE-2018-00048" (p. 13).

### Type of work, in the paper's own words
- Label printed at the top of p. 1: "Review".
- "Therefore, the purpose of this paper is to provide a qualitative literature review of the state-of-the-art technology in this research field, with a primary focus on the safety of workers, the most valuable asset a company can have." (p. 1)
- "In order to find the best results for our research questions, a literature review has been employed as our research methodology." (p. 3)
- "Our systematic review defines a structured process for identifying and analyzing publications. This systematic review is based on clearly formulated research questions (RQ), identifies relevant publications to evaluate, and evaluates the quality of these publications based on explicit criteria." (p. 3)
- "Therefore, it includes the following steps: Identify RQs, identify relevant publications, assess the quality of the publications, summarize the findings, and interpret the findings." (p. 3)
- "The selection criteria were based on a three-step model presented by Kitchenham [8]. These three steps are planning the review, conducting the review, and reporting the review." (p. 3)
- Figure 2 boxes label the output as "studies included in qualitative synthesis" (p. 4).

### Search and selection method
Databases and search arms (p. 3):
- "The literature research took place between February and June 2019, querying Web of Science, Science Direct, and Scopus databases."
- Second arm: "We have also completed the search of the MDPI Publisher Applied Sciences and Sensors journals." Table 1's column heading names these as "Sensors, Applied Sciences, and Symmetry Journals" (p. 4); Figure 2 labels the arm "156 records identified in Sensors Applied Sciences and Symmetry" (p. 4).
- "The review was based on the PRISMA framework" is not the phrasing used; instead: "Figure 2 shows the flowchart of the selection methodology based on the PRISMA flowchart [7]." (p. 3)
- "After formulating the strategy, a rigorous search was conducted in the bibliographic databases, using the pre-selected keywords." (p. 3) The keywords themselves are not printed.

Date window (p. 3):
- "A list of 383 records was identified since 1988."
- "However, to ensure that no outdated sources are used, our literature review focused on the last two, three years. A search interval was set for studies published between January 2017 and June 2019."
- Second arm: "A total of 156 articles were found there since 2017."

Records at each stage, arm 1 (all p. 3):
- 383 identified since 1988 → "Altogether, 198 records were identified" within January 2017–June 2019
- "Duplicates... were then removed. This reduced the search results to 158 specific records."
- "Abstracts of the resulting 158 records were then screened"
- "Elimination of 71 records then occurred due to not being in English, or not having Open Access or Free Downloading rights"
- "Thirty-one records were deemed inappropriate due to being non-industrial studies."
- "two different researchers conducted a full-text article review of the remaining 56 records"
- "Forty records were eliminated for not containing any usable answers to our research questions."
- "The resulting 16 records that have met all eligibility criteria are evaluated in this literature review."

Records at each stage, arm 2 (p. 3):
- "A total of 156 articles were found there since 2017. After reading their titles and their abstracts, eight articles were deemed relevant."

Total included (p. 3): "Sixteen records being from the first search and eight records from the second, 24 publications had been selected as a starting point for further in-depth analysis."

Figure 2 boxes as printed (p. 4): "198 records identified in Web of Science, Science Direct, and Scopus databases"; "156 records identified in Sensors Applied Sciences and Symmetry"; "40 records remaining after removing duplicates"; "108 records excluded"; "158 records screened"; "102 records excluded"; "48 full-text articles assessed for eligibility"; "56 full-text articles assessed for eligibility"; "40 full-text articles excluded"; "40 articles excluded"; "16 studies included in qualitative synthesis"; "8 studies included in qualitative synthesis"; "24 studies included in qualitative synthesis".

Quality criteria: "evaluates the quality of these publications based on explicit criteria" (p. 3); the explicit criteria themselves are not printed.

### Every quantitative or categorical finding (with pages)

Review-level counts:
- Table 1, "Years and number of appropriate publications" (p. 4): 2019 — 4 (WoS/ScienceDirect/Scopus) and 2 (Sensors/Applied Sciences/Symmetry); 2018 — 9 and 5; 2017 — 3 and 1.
- Appendix Table A1, "Summary Table of analyzed publications" (pp. 13–17), 24 rows, columns "Research or Application Area", "Publi-Cation", "Key Technologies", "Main Purpose".
- Categorisation scheme stated in the abstract (p. 1): "These publications were categorized into three subcategories based on the used key technology, research or application area, and their main purposes."
- Three results subsections (p. 5): "the first discussing how AR/VR Smart Glasses may be applied. The second subsection discusses the industrial environment. In the third subsection, usability and sustainability are discussed." Headings: 3.1 "Adoption and Challenges of AR/VR Glasses" (p. 5), 3.2 "Industrial Environment" (p. 8), 3.3 "Usability and Sustainability" (p. 9).
- Three research questions RQ1–RQ3 plus one hypothesis (p. 3).

Numbers reported from the reviewed studies:
- Herz et al. [10]: "research involving 611 consumers from Germany" (p. 5); "consumers show a moderately positive attitude towards VR glasses" (p. 5); "Only the coexistence of these two features resulted in a significant improvement in user attitude." (p. 5)
- Syberfeldt et al. [12]: "reviewed 12 smart glasses" (p. 6); "The battery power was defined to last for an eight-hour shift, the mass was not to exceed 100 g, and the glasses were to be voice-operated (hands-free)." (p. 6); "found the Epson Moverio BT-300 to be the most usable device of the 12 reviewed products" (p. 6); Table A1: "Of the 12 selected smart glasses reviewed, the most suitable and usable was determined based on the analysis of 18 parameters" (p. 13).
- Terhoeven et al. [13]: "a survey involving 59 employees before implementing the technology" (p. 6); Table A1: "Fifty-nine employees were surveyed" (p. 14).
- Mengoni et al. [16]: "An experiment had been carried out with the participation of 30 subjects." (p. 7); two modules: "The Ergonomic Module... The Assembly Support Module" (p. 7).
- Mourtzis et al. [17]: "the results indicated a noticeable improvement in the results of the participants, with errors reduced by 12%, and production assembly time becoming 10% quicker." (p. 8)
- Danielsson et al. [18]: "Thirty-five interviewees were interviewed in a Volvo factory" (p. 8).
- Masoni et al. [19]: "Assuming a 30 Hz frame transmission rate, this severely limits the visual inspection of a machine." (p. 8); repeated p. 12: "Smartphone cameras (with a 30 Hz framerate) are not suitable for all purposes".
- Fang et al. [14]: "a real-time six-Degree of Freedom (DoF) motion tracking" (p. 6); "The constituents of the process are jitter filtering, moderation filtering, and latency filtering." (p. 6)
- Quandt et al. [21]: "The three main categories were requirements during development and integration, during setup, and during operation. The two case studies were wind energy and a welding simulator." (p. 9)
- Yang et al. [23]: "current estimates predict an additional three billion more middle-class consumers by 2030" (p. 9); "a 'take-make-consume-dispose' model" (p. 9); "Two case studies were used to exemplify this proposed remanufacturing process." (Table A1, p. 17); "it offers a production model of nearly 100% recyclable, reusable products." (p. 11)
- Yu et al. [24]: "The authors have recruited 30 participants" (p. 9); "men made up 66% of the participants, whose age group was between 20 and 30 years of age" (pp. 9–10); "four dimensions were being assessed: Hardware, the mobile application, motion sickness (induced by the use of the device), and interaction operation performance" (p. 10); "The results show that motion sickness can be significantly reduced with higher perceived user experience quality." (p. 10)
- Peruzzini et al. [25]: "three purposes of the survey: Avoiding machine downtime (process delay) by improving the process control, ensuring the safety of the workers by preventing excessive mental and physical workloads, and improving the quality of process planning" (p. 10).
- Zheng et al. [26]: "Four transformation methods are enumerated." (p. 10); "the recognition accuracy of 97.2% was achieved for the following eight human activities: Climbing stairs (upwards), climbing stairs (downwards), jumping, lying, jogging, standing, sitting, and walking." (p. 10); Table A1: "a 97.2% recognition rate" (p. 17).
- Han and Kim [27]: "The proposition has been verified, as 45.24% of respondents had confirmed that the hand interaction provided high immersion." (p. 10); Table A1: "45.24% of respondents testing the method reported a high degree of immersion" (p. 17).
- Kim et al. [28]: "Binocular and monocular HWD types, along with the four UIs were separately tested" (p. 10); "Results indicated that UI designs had a greater impact on the previously described criteria, whereas HWD types did not have an observable effect." (p. 10)
- Yildirim and Ali-Eldin [29]: "a survey of 76 employees of an IT firm" (p. 11); "the strongest motivation for using wearable IoT devices is their perceived usefulness" (p. 11).
- Fernández-Caramés et al. [15]: "fog gateways responded quicker when a single Industrial AR (IAR) device was used. However, when additional devices are also operated, this advantage of fog computing diminishes, and cloudlets respond faster." (p. 7); Table A1: "fog computing alone, which proved to handle small payloads (<128 kB) efficiently" (p. 17).
- Zhang et al. [20]: "The testing of the system proved to be successful, verifying the hypothesis." (p. 9); "The three constituents of the system are the following: A 3D sensing module, an in-situ design module, and a Wi-Fi-based communication module." (pp. 8–9)
- Discussion restatement of Yu et al.: "(66% males and 33% females between the age of 20 and 30)" (p. 13).

Definitional / categorical items introduced by the review authors:
- Figure 1, "The types of realities, from real to virtual [4]" (p. 2) with the statement "Each of the three mentioned 'realities' are located on a 'reality-scale'" (p. 2).
- Human factors definition adopted (p. 2): "Human factors refer to environmental, organizational, and job factors, and human and individual characteristics, which influence behavior at work in a way which can affect health and safety".
- "Grant [6] defines the three pillars of the concept of sustainability: Economic (profits), environmental (planet), and social (people)." (p. 2)
- "The focal design principles are the following: Interconnection, collaboration, security, and data analytics [3]." (p. 2)
- Industrial-revolution sequence, first through fourth (pp. 1–2).

### Hypothesis stated, and where confirmed or rejected
**Hypothesis (stated on p. 3):**
> "We have formulated the following hypothesis: VR and AR devices and applications may not yet be suitable for implementations in the industry (still requiring further development and substantial testing)."

**Verdict (stated on p. 13):**
> "We can clearly verify our hypothesis, as most of the proposed systems discussed in this research paper may not be readily available or suitable for use."

Immediately followed on the same page (p. 13) by: "Nevertheless, we anticipate their swift development and adoption in the near future."

Separately, the paper reports a hypothesis belonging to a *reviewed* study, not to the review authors — quoted here so it is not mistaken for the review's own: "The authors' hypothesis was that interaction and the complexity of instructions given to machines can be increased by communication via an AR-based interface." (p. 8, about Zhang et al. [20]), with the reviewed study's verdict "The testing of the system proved to be successful, verifying the hypothesis." (p. 9).

### What the authors say is missing from the field
- "Our greatest concern was that many papers discuss new developments and technological advancements, yet seldom do they discuss human factors and ergonomic design strategies, which also affect productivity and the well-being of workers." (p. 2)
- "The adoptability of new technology, and the burden of learning how to use a new system also may not be the primary focus of papers." (p. 2)
- "The challenges these new devices pose to both the employers and employees should not be neglected." (p. 2)
- "We intend to add a new perspective to the field, emphasizing the importance of safety (resulting in efficiency and increased productivity). Adding this perspective could forward the discussion in the field, improving the technology adopted." (p. 2)
- "Despite this decade experiencing this process of transformation, various fields still require significant development [32]." (p. 11)
- "However, limitations are also noted: Currently available hardware may not be fit for all tasks and stages of industrial production, and hardware manufacturers play a key role in facilitating (or hindering) the widespread adoption of AR/VR technology." (p. 11)
- "This is a major breakthrough in the design of assistive technology, as ergonomics may not have always been prioritized in the past." (p. 7)
- "Manufacturers of smart glasses exclude visually challenged people by not designing their products to be worn by people wearing glasses. This does not adhere to the increasing inclusiveness of modern workplaces, reducing the chances of employment for these people." (p. 12)
- "Changes should be made to safety wear in the future to accommodate AR devices, as current design flaws are impeding the adoption of new technology." (p. 12)
- "Other factors, such as battery life, weight, and handling issues, make these smart glasses unfit for use in a standard eight-hour shift." (p. 12)
- "It is imperative that designers of AR/VR architectures first perform a needs assessment survey before designing new systems, lest the design proves to be ineffective or difficult to use, causing additional problems instead of solving one." (p. 6)

### The authors' own stated limitations and future work
- **Limitations of the review itself: not stated.** No limitations section exists, and no sentence in the paper identifies a limitation of the review's own scope, database coverage, language or access restrictions. (Access restriction is mentioned only as a screening criterion: "not having Open Access or Free Downloading rights (only free research papers were fully accessible)", p. 3.)
- Future work / hopes expressed:
  - "We express the hope that other researchers or managers of industry will be able to use the results of this paper." (p. 3)
  - "We express the hope that further studies will be conducted in this field, which could advance the understanding we have of human factors, and that this research paper will be able to provide the necessary content to facilitate research planning." (p. 13)
  - "Nevertheless, we anticipate their swift development and adoption in the near future." (p. 13)
  - "Manufacturers must provide solutions to these limitations in order to have smart glasses adopted in the future, such as increasing battery life or using lighter materials for the frame." (p. 12)
  - "A needs analysis prior to designing new devices could provide invaluable assistance in designing ergonomic, smart glasses, which could be accepted by its users." (p. 12)
  - "We believe that a possible remedy to this reluctance could be the effective familiarisation of users with the advantages of new AR technology." (p. 12)
  - "We express hope for its imminent widespread implementation" (of the circular economy model) (p. 11)

### Five verbatim sentences (headline claims)
1. "Conclusion: Virtual reality, augmented reality, and IoT are becoming increasingly more suitable for industrial use, despite facing scrutiny and criticism." (p. 1)
2. "We can clearly verify our hypothesis, as most of the proposed systems discussed in this research paper may not be readily available or suitable for use." (p. 13)
3. "Our greatest concern was that many papers discuss new developments and technological advancements, yet seldom do they discuss human factors and ergonomic design strategies, which also affect productivity and the well-being of workers." (p. 2)
4. "Other factors, such as battery life, weight, and handling issues, make these smart glasses unfit for use in a standard eight-hour shift." (p. 12)
5. "Both studies revealed that UI design has a major impact on UX, rather than the device itself." (p. 13)

### Internal inconsistencies
1. **PRISMA flowchart vs text on duplicate removal.** Text (p. 3): "Duplicates, resulting from some entries appearing in multiple databases, were then removed. This reduced the search results to 158 specific records." Figure 2 box (p. 4): "40 records remaining after removing duplicates", placed in the left arm between "198 records identified in Web of Science, Science Direct, and Scopus databases" and "158 records screened".
2. **Second-arm screening stages present in the figure but not in the text.** Text (p. 3): "A total of 156 articles were found there since 2017. After reading their titles and their abstracts, eight articles were deemed relevant." Figure 2 right arm (p. 4) adds: "108 records excluded", "48 full-text articles assessed for eligibility", "40 articles excluded" before "8 studies included in qualitative synthesis".
3. **Which journals formed the second search arm.** Text (p. 3): "We have also completed the search of the MDPI Publisher Applied Sciences and Sensors journals." Table 1 column heading (p. 4): "Number of Publications in the Sensors, Applied Sciences, and Symmetry Journals". Figure 2 (p. 4): "156 records identified in Sensors Applied Sciences and Symmetry".
4. **Announced structure vs actual structure.** Text (p. 2): "In Section 2, the materials and methods are presented. Section 3 deals with the results, while Section 4 discusses them. In the last section, the conclusions are summarized." The paper's last numbered section is "4. Discussion" (p. 11), which runs to p. 13 and is followed directly by "Author Contributions" (p. 13); there is no conclusions section. The word "Conclusion:" appears only as an abstract label (p. 1).
5. **Bogaerts et al. [11] result, three different statements.** Section 3.1 (p. 6): "The human-generated design provided better coverage in the experiments" and "the results were not presented as evidence of humans being superior to computer-generated solutions. Instead, Bogaerts et al. [11] suggested that the quality of automated algorithms may be enhanced by humans, should the problem become more challenging." Discussion (p. 12): "As user-generated layouts are not necessarily better, it is indicated that users can add important knowledge, as they have a better understanding of the problem." Table A1 (p. 16): "The results indicate that the layouts designed by humans were more intuitive and efficient than the automated designs, which also require human interaction, thus proving to be less effective."
6. **Yu et al. [24] sample composition.** Section 3.3 (pp. 9–10): "men made up 66% of the participants". Discussion (p. 13): "(66% males and 33% females between the age of 20 and 30)".
7. **Danielsson et al. [18] finding.** Section 3.2 (p. 8): "a largely positive attitude towards this technology was observed, thus serving as an encouragement to adopt AR." Discussion (p. 12): "A certain degree of reluctance to accept new technology may be observed (Danielsson et al. [18]), but users had a mostly positive attitude towards (AR) devices."
8. **Method label.** Abstract/introduction (p. 1): "a qualitative literature review of the state-of-the-art technology in this research field". Methods (p. 3): "Our systematic review defines a structured process for identifying and analyzing publications. This systematic review is based on clearly formulated research questions (RQ)".
9. **Author-name spelling for reference [10].** Section 3.1 (p. 5): "Herz et al. [10] did research involving 611 consumers from Germany" and "The results discussed in this paper (Herz, 2019) [10]". Table A1 (p. 14): "(Hertz, 2019) [10]". Reference list (p. 18): "Hertz, M.; Rauschnabel, P.A."
10. **Year for reference [14].** Table A1 (p. 16): "(Fang, 2017) [14]". Reference list (p. 18): "Fang, W.; Zheng, L.; Deng, H.; Zhang, H. Real-time motion tracking for mobile augmented/virtual reality using adaptive visual-inertial fusion. Sensors 2019, 17, 1037."

---

## Paper 3 — `AR smart glasses use and acceptance, a literature review (2023).pdf` (14 PDF pages)

### Citation as printed
- **Authors:** George Koutromanos, Georgia Kazakou (p. 1). Affiliation: "National and Kapodistrian University of Athens, Department of Primary Education, Ippokratous 20, 10680, Athens, Greece" (p. 1). Corresponding author: Georgia Kazakou (marked "*"), e-mails "koutro@primedu.uoa.gr", "gkazakou@primedu.uoa.gr" (p. 1).
- **Year:** 2023 (p. 1, running footer "Computers & Education: X Reality 2 (2023) 100028").
- **Full title as printed:** "Augmented reality smart glasses use and acceptance: А literature review" (p. 1). (The character before "literature" is printed as a Cyrillic "А" in the PDF text layer.)
- **Journal:** Computers & Education: X Reality (masthead p. 1; running footer pp. 1–14).
- **Volume:** 2 (running footer).
- **Issue:** not printed.
- **Page range:** not printed. An article number is printed instead: 100028 (running footer). Internal page numbers 1–14 appear at the foot of the pages.
- **DOI:** https://doi.org/10.1016/j.cexr.2023.100028 (p. 1).
- Other printed dates: "Received 9 January 2023; Received in revised form 20 May 2023; Accepted 20 May 2023" (p. 1). Licence: "2949-6780/© 2023 The Authors. Published by Elsevier Ltd. This is an open access article under the CC BY-NC-ND license" (p. 1). Funding: "This research did not receive any specific grant from funding agencies in the public, commercial, or not-for-profit sectors." (p. 11)

### Type of work, in the paper's own words
- Keyword listed: "Review" (p. 1).
- "This study aims to review the empirical research activity concerning the acceptance of ARSGs in all applicable fields." (p. 1)
- "Therefore, this literature review aims to review existing studies on the significant factors of the acceptance of ARSGs among users in diverse fields, identify research gaps, and make proposals for future research." (p. 2)
- "To achieve the purpose of this study, a literature review of studies examining the acceptance or use of ARSGs was conducted." (p. 4)
- "The review was based on the PRISMA framework (Moher et al., 2009)." (p. 4)
- "The study presented a literature review of research on the factors that influence the acceptance or use of ARSGs." (p. 11)

### Search and selection method
Databases and search engine (p. 4):
- "The review was conducted in 2022 through a search on the online databases 'SpringerLink', 'IEEExplore', 'ACM Digital Library', 'Emerald Insight', 'Scopus', 'Web of Science' and the Google Scholar search engine."
- "Only open access databases or databases accessed by the authors' institutional library were utilised." (p. 4)

Date window (p. 4):
- "The search covered the period from 2013 to 2023. The year 2013 was chosen as the starting point of the review because that was when the first smart glasses, Google Glass (Klein et al., 2020), were released."
- Abstract (p. 1): "In total, 21 studies published from 2015 to 2022 have been reviewed."

Search terms (p. 4):
- "The following search terms were used: 'acceptance' OR 'adoption' OR 'acceptance model' AND 'Augmented reality smart glasses' OR 'ARSGs' OR 'Google Glass' OR 'AR Glass'."

Source types (p. 4):
- "Given that ARSG research is still in its infancy, the search focused on journal articles, as well as conference papers and book chapters."
- "Articles refer to studies that have been published in journals, while papers refer to the studies that have been published in conference proceedings or conference proceedings published as book chapters."

Inclusion criteria (p. 4):
- "Journal articles and conference papers should (a) be written in English, (b) refer either to the intention to accept and use the ARSGs or to factors that influence their acceptance and use, and (c) contain empirical data."

Exclusion criteria (p. 4):
- "Articles and papers that could not be accessed, duplicate articles and papers, extended abstracts, short papers, and those whose titles, summaries, keywords, or conclusions were not relevant to the purpose of the review were excluded."

Records at each stage as stated in the text (p. 4):
- "The result of the search yielded 82 articles and papers, excluding duplicates."
- "Then, 51 articles were excluded from the first screening due to irrelevant titles, keywords, or summaries, as well as another two for not being written in English."
- "Afterwards, a full-text eligibility check was conducted. Eight more articles and papers were also excluded as being irrelevant to the aim and research questions of the current review."
- "In total, 21 studies were found to meet the selection criteria. Of these, twelve are articles published in journals, four are conference papers, and another four are conference proceedings published as book chapters."

Records at each stage as printed in Fig. 1, "PRISMA flow diagram for the study (Moher et al., 2009)" (p. 5; the figure is a raster image, transcribed here):
- Identification: "Records identified through an online database search n=83"; "Records identified through targeted search n=4"
- "Records remaining after duplicates were removed n=82"
- Screening: "Records screened n=82"; "Records excluded n=51 irrelevant title/keywords/abstract; n=2 not in English"
- Eligibility: "Full-text records reviewed n=29"; "Records excluded n=2 other technologies; n=2 not empirical data; n= 4 book chapter"
- Included: "Records included in the final analysis n=21"

Coding and quality assessment (p. 4):
- "Initially, the researchers coded and analysed the studies independently, and then differences were resolved through discussion among the researchers."
- "a codebook was developed to extract data from the studies. It included the following elements: the purpose of the study, field, device, acceptance model or theory and its variables, type of research, sample, factors' significant effects, and factors emerging from qualitative studies."
- "To assess their quality, studies should provide certain criteria. These included the provision of a clear purpose, research questions, methodology design, and well-presented results. The results were peer reviewed by the authors to ensure assessment quality."

### Every quantitative or categorical finding (with pages)

Corpus size: 21 studies (pp. 1, 4, 5, 11); Table 1, "Studies on the acceptance and use of ARSGs", rows numbered 1–21 with columns "А/А", "Author/s", "Purpose of the study", "Field and device", "Sample", "Model and variables", "Significant effects" (pp. 6–8); footnote "* = Qualitative study." (p. 8).

RQ1, fields — four categories (p. 5):
- Commercial use: "The first includes 12 studies... that are related to the commercial use of ARSGs. In this category, users of the glasses are treated as customers who would buy the glasses for personal use, i.e., to surf the internet or shoot videos."
- Education: "The second category consists of four studies and refers to the educational use of ARSGs. Three studies were conducted in tertiary education... The fourth was conducted in primary and secondary education".
- Medical: "The third category is related to medical use and includes three studies in the field of medicine and health professionals".
- Other: "The last category includes the use of the ARSGs in other fields, such as tourism and sports. One of them involves museum visitors (Han et al., 2019), while another focuses on cyclists (Berkemeier et al., 2018)."
- Discussion (p. 9): "it was found that almost half of the studies were related to marketing, i.e., the commercial use of ARSGs."

RQ2, theoretical frameworks (p. 5):
- "in cases where the adoption of a particular model or theory was chosen, this model is most often TAM, which is used in four studies in medicine and marketing"
- "In the other five cases where a combination of models or theories was chosen, they were found to include at least TAM..., with the most common case being that of the combination of TAM and UTAUT"
- "Additionally, studies proposing to extend or modify a model based their proposal on TAM" (eight studies cited)
- Abstract (p. 1): "the majority of the studies base their theoretical framework either entirely or in part on the Technology Acceptance Model (TAM)."
- Models described in Section 2.2 (p. 3): TRA, TPB, TAM, TAM 2, TAM 3, DTPB, UTAUT (four determinants, four moderators), UTAUT2 (three added variables: hedonic motivation, habit, price value).

RQ3, research design (p. 5):
- "it was found that most studies used a quantitative methodology" (13 studies cited)
- "Four studies employed a qualitative methodology..., while four others relied on both methodologies"
- Sample composition by field (p. 5): three health professionals/doctors/medical students; three university students; one primary and secondary school teachers; one museum visitors; one professional or amateur cyclists.
- Commercial-use samples (p. 8): "In the remaining 12 studies on the commercial use of smart glasses, the sample consisted of either students or university staff (Adapa et al., 2018; Kalantari & Rauschnabel, 2018), social media users (Nunes & Arruda Filho, 2018; Weiz et al., 2016) or mall consumers (Holdack et al., 2022; Rauschnabel et al., 2015, 2016, 2018; Rauschnabel & Ro, 2016) while in four of them, the sample was not specified".
- Participation categories (pp. 8, 10): "(a) The active participation of the sample... This was the case in only three studies"; "(b) The passive participation of the sample... in the study conducted by the Kazakou and Koutromanos (2022), only the researchers interacted with the glasses. The participants watched this interaction remotely through a ZOOM meeting. In 12 studies, the glasses were presented to the participants through short videos, as well as a list of official images and an explanation of their technical characteristics and basic functions"; "(c) The unspecified participation of the sample. Five studies did not specify whether the glasses were presented or if a demonstration was provided".

RQ4, factors:
- "Seventeen of the reviewed studies utilised a variety of variables to determine the factors that affect the intention to accept or use ARSGs, following either a quantitative methodology or a mixed-methods methodology." (p. 8)
- "Table 1 presents all the variables (independent variables, moderators, and control variables) measured in the context of the 17 studies that used quantitative or mixed methodology and had a statistically significant effect." (p. 8)
- Five categories with study counts (pp. 8–9): "(1) utilitarian factors which appear in 17 studies and refer to a specific gain or benefit, (2) social factors which appear in ten studies and are related to the influence that derives from society, (3) personal factors which appear in ten studies and refer to users' personality traits or psychological characteristics, (4) risk-related factors appearing in eight studies and are related to privacy, health or technology risks, and (5) technological factors which appear in seven studies and refer to specific technological characteristics and functions of ARSGs."
- "This classification was based on Kalantari's review (2017)" (p. 8).
- Total: "51 factors were recorded and grouped into five categories: (1) utilitarian, (2) social, (3) personal, (4) risk-related, and (5) technological factors." (p. 10)
- Table 2, "Factors that affect the intention to accept or use ARSGs" (p. 8), as printed by column (51 listed entries):
  - Utilitarian factors (16): Perceived usefulness; Perceived ease of use; Ease of learning; Ease of reminding; Ease of medical education; Performance expectancy; Hedonic motivation; Perceived enjoyment; Perceived substitutability; Expected functional benefits; Functional benefits; Expected ease of use; Utilitarian benefits; Hedonic benefits; Symbolic benefits; Perceived informativeness
  - Technological factors (8): Functionality; Speech recognition; Ubiquity; Unobtrusiveness; Content awareness; Integration of IS; Documentation; Hands-free
  - Personal factors (12 entries): Motivation; Attitude; Compatibility; Technological compatibility; Brand attitude; Openness; Neuroticism; Extraversion; Attitudes towards the use; Brand attitude; Technology innovativeness; Perception as technology
  - Social factors (9): External influence; Social influence; Image; Descriptive norms; External effects; Expected social conformity; Social norms; Social benefits; Subjective norms
  - Risk factors (6): Trust and privacy; Privacy; Technology risk; Perceived public privacy risks; Other people's privacy; Perceived health risk
- Qualitative-study factors (pp. 9): Adapa et al. (2018) — "25 participants were divided into two groups of users: university students and staff"; "the factor 'look and feel' is very important"; "battery-related issues such as battery life and heating"; "the availability of GPS and messaging, as well as social media applications"; "the hands-free feature, functionality, compatibility, form factor, and battery life". Kazakou and Koutromanos (2022) — "91 teachers of primary and secondary education"; factors "perceived usefulness, compatibility, facilitating conditions (i.e., training on technical and pedagogical issues, supply of educational material, infrastructure and equipment of the school, support from educational leadership, and affordability of purchasing glasses) privacy risk, and potential health risk". Han et al. (2019) — "interviewed 28 visitors of a gallery in the United Kingdom"; proposed model with "four categories of factors: (a) perceived attributes of innovation..., (b) visitor benefits..., (c) visitor resistance..., and (d) social impact". Nunes and Arruda Filho (2018) — "the comments of 86 users of the social network Reddit over a period of four months"; "Three categories and two subcategories of consumers were found: (a) socially satisfied, (b) socially constrained, and (c) early adopters who are distinguished into enthusiasts and visionaries."
- Single-study caveat (p. 10): "For example, only one study identified 'image' as a variable with a statistically significant influence on intention."

Market figures cited (p. 1):
- "in 2019, unit sales of ARSGs totaled 170 thousand units and are expected to rise to 3.9 million units by 2024"
- "the consumer and enterprise ARSGs hardware and software revenue amounted to 1.85 billion U.S. dollars in 2021, while it is forecasted to rise to 35.06 billion U.S. dollars by 2026"

Prior-review figures cited (pp. 2–4, 9):
- "Kalantari (2017) studied 50 articles published from 2009 to 2017"; "recorded a total of 22 factors influencing the acceptance of wearable technologies by consumers, which she grouped into five categories: (1) perceived benefits, (2) technological characteristics, (3) individual characteristics, (4) social factors, and (5) perceived risks" (p. 4)
- "Niknejad's et al. (2020) literature review, which studied 244 studies on the use of smart wearable technologies from 2010 to 2019, found that 58 of them related to the factors that influence the intention to accept or use these technologies. The researchers recorded 38 factors" with "four factors... mentioned as the best predictors" (p. 4)
- "Shin et al. (2019), who reviewed the data from 463 studies on wearable activity tracking" (p. 3)
- Kim and Choi (2021): "by reviewing 57 studies on smart glasses in applied sciences from 2014 to 2020, they identified seven categories of fields and 16 sub-fields" (p. 9)
- "Bower and Sturman (2015) identified 14 affordances" (p. 2)
- Suh & Prophet (2018) two stimulus categories: "(a) sensory... and (b) perpetual" (p. 2)

Sample sizes as printed in Table 1 (pp. 6–8): Adapa et al. (2018) N = 25; Alfaisal et al. (2022) N = 528; AlHamad et al. (2021) N = 429; AL-Maroof et al. (2021) N = 968; Kazakou and Koutromanos (2022) N = 91; Başoglu et al. (2017) N = 81 and N = 122; Başoglu et al. (2018) N = 38, N = 30, N = 71 across three phases; Berkemeier et al. (2018) N = 3 (qualitative) and N = 100 (quantitative); Ernst (2016) N = 109; Göken et al. (2016) N = 8, N = 30, N = 75 across three phases; Han et al. (2019) N = 28; Holdack et al. (2022) N = 143; Kalantari and Rauschnabel (2018) N = 116; Nunes and Arruda Filho (2018) "Reddit users"; Özdemir-Güngör et al. (2020) N = 119; Rauschnabel et al. (2015) N = 146 and N = 201; Rauschnabel and Ro (2016) N = 201; Rauschnabel et al. (2016) N = 266 and N = 1682; Rauschnabel et al. (2018) N = 285 and N = 21; Stock et al. (2016) N = 109; Weiz et al. (2016) N = 111.

Devices as printed in Table 1 (pp. 6–8): Google Glass (rows 1, 2, 3, 4, 14, 17, 21), Epson Moverio AR BT-300 (row 5), "Not specified" (rows 6, 7, 8, 10, 11, 15), Microsoft HoloLens (rows 9, 12, 13, 20), and for rows 18–19 "Microsoft HoloLens, Epson Moverio, Sony SmartEyeglass, Zeis Glasses, Everysight Raptor, Google Glass, ODG R-7".

### Hypothesis stated, and where confirmed or rejected
**No hypothesis is stated.** The paper poses four research questions instead (p. 2):
- "RQ1: In which fields has the acceptance of ARSGs been investigated?"
- "RQ2: What is the theoretical framework of the research, i.e., which acceptance models or theories have been applied?"
- "RQ3: What is the research design of the studies?"
- "RQ4: What are the factors influencing the acceptance or use of ARSGs?"

These are answered in Sections 4.1–4.4 (pp. 5–9) and revisited in Section 5 (pp. 9–10); the four RQs are restated in Section 8 (p. 11). The word "hypothesis" does not appear in the paper.

### What the authors say is missing from the field
- "Since there is no acceptance model for ARSGs, the findings of this review could lead researchers to propose models for accepting ARSGs in various fields, such as education." (p. 1)
- "Hence, there is a need for a review that includes studies focusing particularly on the acceptance of ARSGs." (p. 2)
- "There are currently no studies reviewing the acceptance of ARSGs." (p. 3)
- "Based on the findings of the above literature reviews, two conclusions emerge. First, it is not a given that the factors found to influence the acceptance or use of smart wearables in general also affect the acceptance or use of ARSGs. Second, there is no specific model or theory that explains the acceptance or intention to use smart glasses." (p. 4)
- "Therefore, while ARSGs are being used in a number of fields, it seems that no research has yet been conducted on the factors affecting their acceptance or use." (p. 9)
- "In addition, variables that the research literature has highlighted as determinants of technology acceptance either by individuals or by organisations, such as facilitating conditions (Chong et al., 2022; Khechine et al., 2016), have not been widely measured." (p. 10)
- "Finally, in the models used in the present review, it seems that only factors affecting user intention were examined." (p. 10)
- "Finally, it was found that in acceptance models researchers ignored many of the technological and learning affordances of ARSGs." (p. 10)
- "It seems that established acceptance models lack comprehensive and thorough knowledge of the affordances (e.g., the combination of real and virtual objects, immersive and personalised experience, hands-free access, and first-person view) of smart glasses." (p. 10)
- "The second conclusion is that most of the studies did not give the participants the opportunity to interact with or even wear the glasses." (p. 10)
- "One of the conclusions that can be drawn is that the number of studies investigating the effect of these factors on the acceptance or use of ARSGs is, in many cases, small, so they cannot be characterised as best or promising predictors." (p. 10)
- "However, these reviews did not explore the different fields in which the acceptance of wearable devices was examined, while the present review did so for the ARSGs." (p. 10)

### The authors' own stated limitations and future work
Section 7, "Limitations and future research" (p. 11):
- "This review is limited by two factors. First, only articles and papers written in English were analysed which were either open accessed or available through the authors' institutional library. It is possible that the application of the above research selection criteria precluded research related to the purpose of this literature review. Second, the review focused on specific online databases and search engines."
- "This study could be extended by measuring the factors that were identified by the review (e.g., utilitarian factors, risk factors) within the framework of acceptance models in different contexts. The goal of this measurement would be the proposal of an acceptance model specifically for ARSGs. One field that could benefit from the formation of such an acceptance model would be education. Teacher and student acceptance of ARSGs would be measured in this context."

Future-research statements elsewhere:
- "It is therefore highly recommended that ARSG researchers not only include new fields (e.g., work support, e-commerce) in their interests, but also expand their research to fields that have already been researched." (p. 9)
- "However, especially for ARSGs, the research community should consider using both quantitative and qualitative data collection tools more often." (p. 9)
- "In this case, it is suggested that future researchers consider such issues not only in their questionnaires, but also through interviews and user observation." (p. 9)
- "Future research needs to conduct a more larger scale investigation, and rely on different contexts, including cultural or professional, to determine which of the above factors can be utilised for the theoretical foundation of the acceptance of ARSGs." (p. 10)
- "Future research should be expanded to also examine whether intention influenced the purchase or use of ARSGs." (p. 10)
- "These affordances give ARSGs a relative advantage over other mobile technology devices and should be considered in future adoption models." (p. 10)
- "Finally, the present review provides new directions for future research in the field. These are the inclusion of new fields in the research on the acceptance of ARSGs, integration of technological characteristics and affordances of ARSGs into acceptance models, hands-on experience of smart glasses provided via interaction with them, use of qualitative methodological design to record the experiences of ARSG users when measuring their acceptance, as well as measurement of the factors identified in this review on larger samples with a diverse professional and cultural background." (p. 10)
- "(d) further research is needed to identify the factors that could be described as best or as promising predictors of the acceptance of ARSGs." (p. 11)
- Model-content proposals for future models are given on p. 10 (perceived risk, perceived trust, price value from UTAUT2, hands-free for industry/medicine/sports, immersion and presence for tourism and culture, facilitating conditions and relative advantage for education).

### Five verbatim sentences (headline claims)
1. "It was found that the research focuses more on users acting as consumers of the glasses than on professionals (e.g., doctors, teachers, athletes) using them in a specific field." (p. 1)
2. "There are currently no studies reviewing the acceptance of ARSGs." (p. 3)
3. "Regarding the fourth research question - the factors influencing the acceptance or use of ARSGs - 51 factors were recorded and grouped into five categories: (1) utilitarian, (2) social, (3) personal, (4) risk-related, and (5) technological factors." (p. 10)
4. "The second conclusion is that most of the studies did not give the participants the opportunity to interact with or even wear the glasses." (p. 10)
5. "The main contribution of this review is that it fills the existing research gap regarding the acceptance of ARSGs, as it is one of the first to address their acceptance or use." (p. 11)

### Internal inconsistencies
1. **Publication-type breakdown does not sum to the corpus.** "In total, 21 studies were found to meet the selection criteria. Of these, twelve are articles published in journals, four are conference papers, and another four are conference proceedings published as book chapters." (p. 4) — 12 + 4 + 4 = 20.
2. **Book chapters included vs excluded.** Text (p. 4): "another four are conference proceedings published as book chapters" (describing included studies), and (p. 4) "the search focused on journal articles, as well as conference papers and book chapters." Fig. 1 (p. 5) lists among the eligibility-stage exclusions: "n= 4 book chapter".
3. **Reason for the eight full-text exclusions.** Text (p. 4): "Eight more articles and papers were also excluded as being irrelevant to the aim and research questions of the current review." Fig. 1 (p. 5): "Records excluded n=2 other technologies n=2 not empirical data n= 4 book chapter".
4. **The second search arm and the 29-record full-text stage appear only in the figure.** Fig. 1 (p. 5): "Records identified through targeted search n=4" and "Full-text records reviewed n=29". The methodology text (p. 4) states only "The result of the search yielded 82 articles and papers, excluding duplicates" and does not mention a targeted search or the 29 figure.
5. **Search window vs date the review was conducted.** "The review was conducted in 2022 through a search on the online databases... The search covered the period from 2013 to 2023." (p. 4) Abstract: "In total, 21 studies published from 2015 to 2022 have been reviewed." (p. 1)
6. **Rauschnabel et al. (2018) sample described two ways in one sentence, and a third way in Table 1.** Page 8: "the sample consisted of either students or university staff..., social media users... or mall consumers (Holdack et al., 2022; Rauschnabel et al., 2015, 2016, 2018; Rauschnabel & Ro, 2016) while in four of them, the sample was not specified (Bas¸oglu et al., 2017; Ernst, 2016; Rauschnabel et al., 2018; Stock et al., 2016)." Table 1, row 19 (p. 8): "1st study N = 285 students 2nd study _ N = 21 the status of the participants is not specified".
7. **Commercial-use sample attributions exceed the stated 12 studies.** "In the remaining 12 studies on the commercial use of smart glasses..." (p. 8) is followed by 2 + 2 + 5 named studies and "in four of them, the sample was not specified" (p. 8), i.e. 13 study slots across the two lists.
8. **A statistically significant factor from Table 1 is absent from Table 2.** Table 1, row 18 (Rauschnabel et al., 2016) lists among the significant effects "4) PAF→PI" (p. 7), where PAF is defined in the same row as "Perception as Fashion (PAF)". Table 2 (p. 8) lists "Perception as technology" but no "Perception as fashion".
9. **A factor is listed twice in the same Table 2 column.** "Brand attitude" appears twice in the "Personal factors" column of Table 2 (p. 8), which is the column that yields the stated total of 51 factors (p. 10).
10. **Factor name differs between tables.** Table 1, row 8 (Berkemeier et al., 2018) defines "Context Awareness (CAW)" (p. 6); Table 2 lists "Content awareness" (p. 8).
11. **Undefined abbreviations in the Significant effects column.** Table 1, row 12 (Holdack et al., 2022) defines "Behavioural intention to use (BI), Perceived (PU), Perceived ease of use (PEOU)... Perceived enjoyment (E), Perceived informativeness (PINF)" but its significant effects are given as "1) AT→BI 2) PU→AT ... 6) PEOU→PI 7) PEOU→PE ... 10) PI→PE 11) PI→PU" using AT, PI and PE, which are not defined in that row (p. 7). Table 1, row 10 (Göken et al., 2016) lists "3) C→U" where "U" is not defined (p. 6). Rows 7, 10, 12, 13, 15 print the variable as "Perceived (PU)", omitting "usefulness" (pp. 6–7).
12. **Study classified as quantitative but described as involving interviews.** Section 4.3 lists Holdack et al. (2022) among the studies that "used a quantitative methodology" (p. 5); Section 4.3's participation discussion states "(a) The active participation of the sample. This means that the participants interacted with the ARSGs and were subsequently interviewed. This was the case in only three studies (Adapa et al., 2018; Han et al., 2019; Holdack et al., 2022)." (p. 8)agentId: add2cc108ee402c15 (use SendMessage with to: 'add2cc108ee402c15', summary: '<5-10 word recap>' to continue this agent)
<usage>subagent_tokens: 188774
tool_uses: 34
duration_ms: 643682</usage>
---

# PART 5 — Frizziero 2019, Frizziero 2022, Cantarelli

All three PDFs extracted page by page with pymupdf. Below is the evidence profile. Page numbers are PDF page numbers (1-indexed). Ligature glyphs (fi, ff, ffi) in quoted text are transcribed as ordinary letters.

---

# Paper 1 — `Design for disassembly and AR, gearbox case study (2019) · Machines.pdf` (26 PDF pages)

## Citation as printed
- **Authors:** Leonardo Frizziero, Alfredo Liverani, Gianni Caligiana, Giampiero Donnici, Luca Chinaglia (p. 1)
- **Year:** 2019. Printed on p. 1: "Received: 6 March 2019; Accepted: 12 April 2019; Published: 1 May 2019"
- **Full title:** "Design for Disassembly (DfD) and Augmented Reality (AR): Case Study Applied to a Gearbox" (p. 1)
- **Journal:** *Machines* (masthead p. 1; running header on pp. 2–26 reads "Machines 2019, 7, 29")
- **Volume:** 7 (running header, p. 2 onward)
- **Issue:** not printed. The DOI string encodes it ("machines**702**0029") but no issue number appears in text or header.
- **Article number:** 29 (p. 1 footer: "Machines 2019, 7, 29; doi:10.3390/machines7020029")
- **Page range:** no journal page range printed; running footers count "2 of 26" … "26 of 26", i.e. 1–26 of the article PDF.
- **DOI:** 10.3390/machines7020029 (p. 1)
- Affiliation: Department of Industrial Engineering, Alma Mater Studiorum University of Bologna (p. 1). Funding: "This research received no external funding." (p. 25)

## Type of work, in the paper's own words
- Label above the title: "Article" (p. 1).
- p. 1: "The present paper aims to introduce a new procedure for improving maintenance processes in industrial engineering and production."
- p. 8: "A case study is now illustrated in order to better understand and apply all the concepts presented up to now."
- Section headings: "2. Materials and Methods (DfD and AR)" (p. 2), "3. Case Study: Application of Augmented Reality" (p. 20), "4. Discussion and Conclusions" (p. 25).

## Object being disassembled, and how many parts/steps
- "The gearbox represented is the RV 250 screw gearbox of the Rossi Motoriduttori Soc. of Modena" (p. 8). A worm/screw speed gearbox, modelled in Solidworks 2016 (p. 8).
- Target component: "We wanted to perform a disassembly that allowed us to remove the drive shaft with an endless screw, which was considered our target component." (p. 11)
- Bill of materials (Table 1, p. 9): **32 numbered BOM entries**. The paper does not print a total part count. Summing the printed Quantity column gives 79 individual pieces (my count of the printed table; not stated by the authors).
- Partition used for the second method (Table 3, pp. 12): 17 component codes C1–C17 (21 rows with SX/DX duplicates) and 13 fastener codes F1–F13 (15 rows with SX/DX duplicates).
- Step counts are **not stated** by the authors. Counting the printed sequences: first method 37 entries (pp. 11–12), second method 36 entries (p. 16), authors' sequence 32 entries (p. 16).

## Where the disassembly sequence comes from, and where AR enters
Three sequences, all produced by non-AR methods, before any AR work:

1. **First method** — Yi, Yu, Du, Li, Hu, "Research on the Selectable Disassembly Strategy of Mechanical Parts Based on the Generalized CAD Model"; disassembly-wave / removal influence graph (RG). "This first method, introduced by Jianjun Yi, Bin Yu, Lei Du, Chenggang Li, and Diqing Hu, defines the order in which the components are removed to disassemble a selected component, called Cx." (p. 3). Applied on pp. 11–12: "The disassembling sequence obtained is as follows: 16 SX INF, 16 DX INF, 12, 17, 15, 26 DX, …, 25, 24." (p. 11)
2. **Second method** — Tao, Bi, Zuo, Nee, "Partial-Parallel Disassembly Sequence Planning for Complex Products"; disassembly precedence matrix (DPM) built from CFM/FCM/CCM/FFM submatrices plus Rules 1–3 and a five-step generation procedure. "This method, presented by Fei Tao, Luning Bi, Ying Zuo and A.Y.C. Nee, allows the planning of partial and/or parallel disassembly sequences using a DPM (disassembly precedence matrix)" (p. 5). Applied pp. 12–16.
3. **Authors' own sequence** — no algorithm. "The sequence hypothesized by the authors was realized on the knowledge of the gearbox that was acquired during the three-dimensional modeling phase. Authors relied mainly on intuition and common sense in addition to experience." (p. 16)

Selection among the three was by an estimated-time criterion (pp. 16–17), then by economic analysis (pp. 17–18).

**AR enters after the sequence is chosen.** Explicit statements:
- p. 1: "DfD finds the best sequence for disassembling the specific assembly, while AR can demonstrate, in a 3D modality, how the abovementioned sequence is embodied in the industrial process, thereby helping workers during maintenance processes."
- p. 2: "the best sequence is chosen and implemented in CAD; finally, the same best sequence is implemented in the AR environment."
- p. 21: "the authors realized, in the AR environment, the representation of the disassembling sequence that was found to be faster among those obtained in the case study of the speed gearbox. To do this, the authors took advantage of some of the potential offered by Unity and Vuforia."
- p. 23: "the authors created an animation of the GameObject (that is, the speed gearbox) that represents the fastest disassembling sequence".

**Explicit status:** the display did **not** plan the sequence. AR (Unity + Vuforia, ImageTarget marker, keyframe animation, VirtualButton with a C# script in Visual Studio 2017, pp. 21–24) only visualised a sequence already produced by DfD methods and already scored by the time criterion. The paper reports no AR-based validation measurement either.

## Every quantitative result, with PDF page number
**Time estimates (p. 17).** Unit and conversion as printed: "it was sufficient to multiply the value in TMU by 0.036 (1 TMU = 0.036 s)". TMU = time measurement unit (p. 17).
- First method (selectable disassembly strategy): 9415 TMU = 338.94 s (p. 17)
- Second method (partial–parallel): 9324 TMU = 335.66 s (p. 17)
- Authors' sequence: 8255 TMU = 297.18 s (p. 17)

**Table 5, "Results and comparison among the sequences studied and applied" (p. 18):**

| Sequence | Time (min) | Time (h) | Components Worked for One Year (pcs/Year) | Unit Cost (€/pcs) | Cost to Produce 16991 pcs/Year (Case Less Productive) | Annual Savings (€/Year) |
|---|---|---|---|---|---|---|
| Selectable disassembly strategy | 5.65 | 0.094 | 16,991 | 2.02 | 34,321.82 | 0 |
| Partial–parallel disassembly | 5.59 | 0.093 | 17,174 | 2.00 | 339.8200 | 339.82 |
| Authors sequence | 4.95 | 0.083 | 19,393 | 1.77 | 30074,07 | 4247,75 |

(Table 5 gives times in minutes and hours only; the seconds values and the 0.036 s/TMU conversion are on p. 17.)

**Labour cost inputs (p. 18):** "Average annual cost = €34,347.52; average hourly cost = €21.47; theoretical annual hours = 40 h * 52.2 weeks = 2088; hours worked average per year = 1600." Source stated as Italian Ministry of Labour and Social Policies data "updated to October 2017", worker "hired at the 4th level indefinitely" (p. 17).

**Savings statement (p. 18):** "in our case, we can save over €4000/year".

**Gearbox construction data (Table 2, p. 10):** z1 = 1; z2 = 42; mn = 9.9226 mm; mx = 10 mm; dm1 = 80 mm; dm2 = 420 mm; da1 = 99.84 mm; da2 = 439.84 mm; α = 20°; pn = 31.157 mm; pa = 31.416 mm; β = 7°8′; a = 250 mm.

**AR implementation numbers:** ImageTarget "used dimensions: 512 × 885" (p. 21); AR views at scale 1:1 (pp. 22–23, 24), 1:4 (p. 24), 1:10 (p. 25).

**Note on units:** the only unit conversion in the paper is TMU → seconds (0.036 s per TMU, p. 17). Table 5 restates the same three times in minutes and hours; both values are listed above. No percentage results are reported.

## What the authors attribute the improvement to
Attributed to sequence design and to reduction of the number of elements disassembled — not to AR:
- p. 17: "The sequence obtained with the first method is the longest since it has led to a complete disassembly of the gearbox, unlike the other two which, by contrast, are faster because we reduced the number of elements to be disassembled."
- p. 16: "During the disassembly, the authors tried to perform in succession the removal of components and/or fasteners that required the same work tool or that, depending on the case, were located on the same side of the gearbox, thus reducing the running time and fatigue to be done."
- p. 18: "It can be seen how a good design of the disassembling sequence allows having no indifferent advantages on the economic side (in our case, we can save over €4000/year)".
- p. 25: "the authors had the opportunity to deepen and touch on how the design for disassembly is able to offer advantages from different points of view, allowing a better management of resources, in particular, the times and costs related to disassembly."

What is attributed to AR (unquantified, no measurement):
- p. 25: "The possibility to realize an animation and to manage it through the interaction with a virtual button offers concrete advantages in the speed and quality of learning, supporting the operator in the process of disassembling."
- p. 25: "Applying it to the case study, the authors were able to gain a better understanding of the real dimensions of the gear in question, as well as the structure of the object as a whole and in each of its parts."
- p. 20 lists claimed generic industrial AR advantages ("reduction of human error in the execution of the various operations, improvement of efficiency, and reduction of time and costs related to maintenance and production and disposal of products [15]") — these are attributed to the literature reference [15], not measured here.

## Human participants; task time or error rate measured on people
**No human participants.** The word "participant" does not occur. All times are indirect desk estimates from published TMU tables: "The method adopted consists of an indirect estimate of the times." (p. 16) and "a quantitative assessment was provided by assigning scores in TMU (time measurement unit). For this phase, the tables in Anoop Desai and Anil Mital's essay [9] were taken as a reference." (p. 17). No task time or error rate was measured on people. The paper contains **no sentence** stating that such metrics were not collected — this is absence, not a stated disclaimer.

## Authors' own limitations, difficulties, future work, and authoring effort
- Authoring effort of the AR content (p. 25): "authors also encountered some disadvantages, noting, for example, that the time required to create the animation increases quickly if you need to consider many components or if you want to add some features to the pieces in addition to the simple movement (for example, making them disappear after being removed or changing color while moving)."
- Programming skill (p. 25): "The greatest difficulty the authors encountered is the one related to programming, because if you want to develop further, more elaborate but also more advantageous functions (such as the possibility to pause the animation), a certain dexterity in programming is required, surely facilitated if you have some experience in this regard, which for them was quite limited."
- Toolchain friction (p. 21): "Unfortunately, with Solidworks 2016, it was not possible to directly convert the components to the desired format, which is why the authors first worked with Solidworks to convert the assembly file from the sldasm format to the 'step' format and then, using the three-dimensional modeling program PTC Creo Parametric 3.0, we performed the conversion of the single components".
- Sequence not claimed optimal (p. 18): "the sequence conceived by the authors (the most advantageous) is not said to be the best ever: If we continued to search for other possible disassembling sequences, we would probably have reached a sequence that was even more advantageous than this".
- Method scalability (p. 6, about the second method): "obtaining these results in the case of a complex product is long and demanding, which is why it would be necessary to use a computer that is able to perform the operations automatically."
- Maturity of AR (p. 25): "this technology offers considerable advantages in many areas, although it is not yet fully widespread and exploited, which is why in the years to come, we expect continuous improvements in the potential associated with it."
- No dedicated "Future work" section; no limitations section.

## Five verbatim headline sentences
1. "DfD finds the best sequence for disassembling the specific assembly, while AR can demonstrate, in a 3D modality, how the abovementioned sequence is embodied in the industrial process, thereby helping workers during maintenance processes." (p. 1)
2. "It can be seen how the sequence obtained with the first method corresponds to a value of 9415 TMU (corresponding to 338.94 s) and how this value is the highest among the three sequences made: In fact, the sequence obtained with the second method had a score of 9324 TMU, which corresponds to 335.66 s, while the sequence devised by the authors had a score of 8255 TMU, corresponding to 297.18 s." (p. 17)
3. "It can be seen how a good design of the disassembling sequence allows having no indifferent advantages on the economic side (in our case, we can save over €4000/year)". (p. 18)
4. "In order to obtain greater advantages from augmented reality, the authors created an animation of the GameObject (that is, the speed gearbox) that represents the fastest disassembling sequence, thus creating an animation that helps to understand the sequence of disassembly in a better and more rapid manner." (p. 23)
5. "The possibility to realize an animation and to manage it through the interaction with a virtual button offers concrete advantages in the speed and quality of learning, supporting the operator in the process of disassembling." (p. 25)

## Internal inconsistencies
1. **Table 5, "Cost to Produce" cell for the partial–parallel row (p. 18).** The cell reads "339.8200" while the same row prints "Unit Cost (€/pcs) 2.00" and "Components Worked for One Year 17,174", and the reference quantity in the column header is "16991 pcs/Year"; 2.00 × 16,991 = 33,982.00, and the row's own "Annual Savings" cell reads "339.82" (34,321.82 − 33,982.00 = 339.82). The cost cell therefore carries the savings figure's digits rather than a cost.
2. **Citation numbering for the second method.** p. 5: "This method, presented by Fei Tao, Luning Bi, Ying Zuo and A.Y.C. Nee, allows the planning of partial and/or parallel disassembly sequences using a DPM (disassembly precedence matrix) … This method considers a nondestructive disassembly of components and fasteners [12]." But reference [12] on p. 26 is "Fargione, G.; Giudice, F. Pianificazione del disassemblaggio di sistemi meccanici: Approccio mediante algoritmi genetici." while Tao et al. is reference [10]: "Tao, F.; Bi, L.; Zuo, Y.; Nee, A.Y.C. Partial/Parallel Disassembly Sequence Planning for Complex Products. J. Manuf. Sci. Eng. 2018, 140, 011016." Likewise p. 4 cites the first method's worked example as "[11]" and reference [11] (p. 26) is indeed Yi et al., while p. 3's pointer to "two algorithms of DSP" is "[10]" (Tao et al.).
3. **Text vs Table 4 on direction encoding.** p. 13 states: "If, for example, we want to represent the possibility of removing a component along the +x direction, we would write 010000 (Table 4)." Table 4 on the same page lists "−x 100000 / +x 010000", agreeing; but Table 3a (p. 12) assigns C1's removal direction as "/" and C14 as "110000" (two directions), which the text's single-direction description does not cover. This is a gap in the description rather than a contradiction between two printed values.
4. **Minor arithmetic drift in Table 5 (p. 18):** the printed pcs/Year values reproduce exactly only for the first row (1600 h × 3600 / 338.94 s = 16,991); for the other two rows the printed 17,174 and 19,393 differ slightly from the values implied by 335.66 s and 297.18 s.
5. **Sub-section numbering:** p. 5 heading reads "2.1.3. 2nd Method" and p. 3 heading reads "2.1.2. 1st Method" — consistent; no numbering break found.

Otherwise: none found (TMU-to-second conversions on p. 17 are all internally consistent at 0.036 s/TMU, and the three step-sequences on pp. 11–16 are consistent with the BOM numbering of Table 1).

---

# Paper 2 — `Disassembly sequence planning validated thru augmented reality for a speed reducer.pdf` (32 PDF pages)

## Citation as printed
- **Authors:** Leonardo Frizziero, Giampiero Donnici, Gian Maria Santi, Christian Leon-Cardenas, Patrich Ferretti, Gaia Pascucci, Alfredo Liverani (pp. 1, 2)
- **Year:** 2022 ("Published online: 28 Apr 2022"; "Received: 17 November 2021 / Accepted: 01 March 2022", p. 2)
- **Full title:** "Disassembly sequence planning validated thru augmented reality for a speed reducer" (pp. 1, 2)
- **Journal:** *Cogent Engineering* (p. 1; running footer on all pages "Frizziero et al., Cogent Engineering (2022), 9: 2061321"). ISSN 2331-1916 (Online) (p. 1); "published by Cogent OA, part of Taylor & Francis Group" (p. 32)
- **Volume:** 9 — **Issue:** 1 (p. 1, self-citation block: "Cogent Engineering, 9:1, 2061321")
- **Article number:** 2061321
- **Page range:** running footers "Page 1 of 31" (PDF p. 2) through "Page 31 of 31" (PDF p. 32), i.e. 1–31. PDF p. 1 is the unnumbered tandfonline cover sheet.
- **DOI:** 10.1080/23311916.2022.2061321 (pp. 1, 2)
- Section label: "PRODUCTION & MANUFACTURING | RESEARCH ARTICLE" (p. 2). Reviewing editor: Wenjun Xu, Wuhan University of Technology (p. 2). "The authors received no direct funding for this research." (p. 29)

## Type of work, in the paper's own words
- "PRODUCTION & MANUFACTURING | RESEARCH ARTICLE" (p. 2)
- p. 2 (abstract): "The methodology proposed compares three probable disassembly sequences following a comparison of literature-found procedures between genetic algorithms and as a 'state space search' problem, followed by a hybrid approach developed by the authors."
- p. 5: "This research work develops a first state of the art review on DSP is presented, with an in-depth study of the main methods present in the literature, two models of which will then be applied to the case study of the speed reducer."
- p. 2 (Public Interest Statement): "The study … was performed to help to better understand the disassembly analyses in a practical way, by performing a digital model-based simulation of the calculated disassembly sequences by means of Augmented Reality (AR) technology."

## Object being disassembled, and how many parts/steps
- "In this project, a parallel axis speed reducer is studied, in which there are three shafts that house a total of four cylindrical helical gears" (p. 13). Modelled in "PTC Creo Parametric—student limited license" (p. 14).
- "The casing is made up of two pieces held together by a series of screws. The twelve screws are of three different types, distinguished by the diameter." (p. 13); "The three shafts at the ends are wedged inside six bearings" (p. 13).
- **BOM (Table 1, pp. 15–16): 23 codes.** Summing the printed Quantity column gives 34 individual parts (my count; total not printed). Screws: 2 × M6x20 + 4 × M8x50 + 6 × M10x20 = 12, matching the text.
- Codes with two instances are split in the matrices as 6a/6b and 18a/18b (Tables 2, 4, pp. 16, 18).
- Step counts as printed in Table 8 (p. 28): PSO sequence 24 entries; GA sequence 23 entries (including subset S1); customized sequence 18 entries (including subsets S1, S5, S19). The time chart Table 7 (pp. 21–23) itemises 33 tasks (1a–1l, 2a, 3a–3f, 7a–7b, 4a–4f, 5a–5c, 6a–6c).

## Where the disassembly sequence comes from, and where AR enters
Three sequences, all produced before AR:

1. **First sequence — Particle Swarm Optimization + Dijkstra on a state-space search.** p. 9: "Moreover, the first sequence approaches the sequential, manual work Disassembly Sequence Planning (DSP) problem with a PSO, method that literature findings shown to be first presented by y Eberhart and Kennedy in 1995". Same page: "Optimization method: Dijkstra's algorithm (DA)—A typical algorithm to solve the shortest path problem"; "Presentation of the disassembly sequence, which also includes fasteners, based on Particle Swarm Optimization (PSO)." Also p. 9: "Moreover, the disassembly sequence was obtained as a 'state space search' problem." and p. 15: "Furthermore, Dijkastra's algorithm was used to solve the status space search problem." Inputs: component-fastener graph (Fig. 8, p. 16), initial interference matrix (Table 2, p. 16), fastener incidence matrix (Table 3, p. 17), penalty matrix (Table 4, pp. 18–19).
2. **Second sequence — block-based genetic algorithm (Tseng et al., 2018).** p. 12: "Afterwards, the second sequence is taken from the research of Tseng et al. (Tseng et al., 2018), which proposes a block-based GA for the planning of disassembly sequences, based on the comparison between the Kongar and Gupta genetic algorithm and the Dijkastra algorithm." p. 17: "The second disassembly sequence was gathered by following the GA based on the concept of blocking performed by Tseng et al. (Tseng et al., 2018), it proved to be a reasonably quick method to implement manually".
3. **Third ("Alternative"/"Customized") sequence — authors' manual reasoning.** p. 24: "An additional disassembly sequence was gathered; by thinking if physically the real mechanical assembly had to disassemble and thought about what it would be necessary to minimize the time. Some technical aspects were taken such as minimizing the number of times the tool is changed, the number of movements of the operator, the number of movements of the assembly and the convenience in removing a component, therefore the space available for maneuver it".

**AR enters after all three sequences are computed and time-scored.** Explicit statements:
- p. 6: "AR would become the main tool that helps designers to technically validate the process that has been demonstrated thru the calculated sequences."
- p. 2 (abstract): "Time and evaluation of these procedures reached to the best performing sequence. A subsequent augmented reality disassembly simulation was performed with the top-scored operation sequence".
- p. 2 (Public Interest Statement): "AR technology in this study is a tool used to better understand the matters of each proposed sequence arrived from a theoretical foundation."
- Section 2.1.5 "Augmented reality application" (p. 27) follows Section 2.1.4 "Time and Evaluation Calculation" (p. 26). Implementation: Unity + Vuforia, model exported from Creo Parametric as .obj, animation keyframes on the "complete_driver" Game Object (pp. 27–28).
- p. 28: "Moreover, an AR application of the most optimal sequence for disassembly was performed. … This approach allowed to technically realize the sequences calculated by each methodology". "Part interference was verified for each sequence." (p. 28)

**Explicit status:** the display did **not** plan the sequence. AR displayed and (in the authors' wording) "validated"/"technically realize[d]" sequences already planned by PSO/Dijkstra, block-based GA, and manual reasoning. The validation reported is geometric ("Part interference was verified for each sequence.", p. 28), not a human-performance measurement.

## Every quantitative result, with PDF page number
**Evaluation function, first sequence (Table 5, p. 19; text p. 17):** "The total evaluation function was gathered by adding these values which in this case was equal to 7." Table 5's per-step values sum to 7, and its "Chosen Sequence" column reads C23, C22, C21, C13, C5, C8, C6b, C17, C18b, C15, C11, C20, C4, C7, C19, C10, C14, C6a, C18a, C16, C9, C2, C3, C1 (24 steps).

**Fitness function, second sequence (p. 20):**
- "Taking M = 3 therefore 3 feasible disassembly sequences were used to calculate the fitness function"
- "Also the fitness function could decrease by about five points on average with the crossover mechanism alone."
- "For Sequence 1 it was not possible to change the position of any element, and the value of the fitness function remained 17."
- "With the sequence Offspring 3 It was able to modify the position of two components, decreasing the value of the fitness function by 4, arriving at 11."

**Penalty scoring rules (p. 17):** "For a 90 ° change of disassembly direction a score of 1 is assigned, for a 180 ° change a score of 2 is assigned." / "For the disassembly method, a score of 1 is assigned for the tool change."

**Time results — Table 7 total (p. 23) and Table 8 (p. 28).** Unit and conversion as printed in both tables: "Unit value (TMU) 0.036". The paper does not print the words "1 TMU = 0.036 s"; the column is labelled "Estimated time (ET) (sec.)".

| Model | Total time (TMU) | Unit value | ET (sec.) | ET in minutes |
|---|---|---|---|---|
| PSO Method | 480.7 | 0.036 | 173.052 | 2.8842 |
| GA Method | 485.0 | 0.036 | 174.60 | 2.910 |
| Customized | 478.5 | 0.036 | 172.26 | 2.871 |

(Table 8, p. 28. The Customized row is repeated as the total of Table 7, p. 23: "Total time (TMU) 478.5 / Unit value (TMU) 0.036 / Estimated time (ET) (sec.) 172.26 / ET in minutes 2.871".)

Sequences printed in Table 8 (p. 28):
- PSO: C23-C22-C21-C13-C5-C8-C6b-C17-C18b-C15-C11-C20-C4-C7-C19-C10-C14-C6a-C18a-C16-C9-C2-C3-C1
- GA: S1-C13-C6A-C18A-C7-C9-C6B-C18B-C11-C2-C8-C15-C20-C5-C4-C14-C16-C19-C10-C17-C3-C1-C12
- Customized: S1-C13-C4-S5-C8-C15-C14-C16-C18a-S19-C18b-C17-C11-C10-C9-C1-C2-C3 (also printed at p. 26 with "S1, that contains: C21, C22, C23; S5 that contains C5, C6a, C6b, C7; and S19 including C19, C20")

**Both units for the time results, as printed:** minutes and seconds are both given in Table 8 (p. 28): 2.8842 min = 173.052 s; 2.910 min = 174.60 s; 2.871 min = 172.26 s. The stated conversion basis is the TMU unit value of 0.036 (see inconsistency 1 below). Text summary, p. 27: "It could be seen that an assessment of the shortest total time related to the customized sequence as shown in Table 7, and equal to 2.87 minutes, which differs from the other proposals by tenths or hundredths of a minute." Repeated p. 28: "The sequence with the lowest value was the customized sequence, with ET equal to 2.87 minutes, which differs from the other proposals by tenths or hundredths of a minute."

**Per-task TMU scores (Table 7, pp. 21–23):** 12 screw removals at 12 each; top cover 19.8; six covers 14.8, 13.8, 13.8, 14.6, 13.8, 13.8; sealing rings 20.7 and 20.2; bearings 21, 20.7, 20.7, 21.5, 20.7, 20.7; spacers 9.2, 9, 9.3; shafts 12.8, 11.8, 11.8. (These sum to the printed 478.5.)

**Interference matrix values (Table 2, p. 16):** e.g. Shaft 1 = 16, Shaft 2 = 15, Shaft 3 = 16, Bottom carcass = 33, Upper carcass = 12, bearings 14 / 1–4, end covers and screws 0.

**Figures cited from the literature (introduction, not results of this study):** "The waste materials of manufacturing companies, which account for 70% of environmental pollution (Lambert, 2003)" (p. 3); "optimizing the disassembly process has shown to result in 10–20% of all disassembly-related earnings. Most of the disassembly related gains (80–90%) tend to be determined in the product design phase (Desai & Mital, 2003)." (p. 4).

**No AR-related quantitative result is reported anywhere in the paper.**

## What the authors attribute the improvement to
Attributed to the sequence itself, produced by combining the two literature methods — not to AR:
- p. 29: "The study of the disassembly sequences let to be clear about the critical issues related to the design of the disassembly of a component, therefore a customized sequence could be created by means of combining two different theories. This allowed to reach the best sequence for disassembly times, noting significant deviations. the differences of which are negligible due to the low complexity of the speed reducer, which has a Bill of Materials limited to a few types of components."
- p. 24: "Some technical aspects were taken such as minimizing the number of times the tool is changed, the number of movements of the operator, the number of movements of the assembly and the convenience in removing a component".
- p. 25: "Following the philosophy of minimizing tool change, all six bearings were taken out from the shafts in the assembly."

What is attributed to AR (unquantified, no measurement):
- p. 29: "As for disassembly, at this point it is intuitive to imagine a digital manual, with which the user is better able to familiarize himself with the assembly than a traditional paper manual. Seeing the product in augmented reality, it was much easier to perceive the three dimensions, the dimensions of the components, their joints, and the operations to be performed in sequence."
- p. 28: "By looking at the AR simulation, it could be noticed how the chosen design showed a flexible approach for assembly and disassembly".
- p. 29: "In maintenance, remote assistance and preventive and predictive maintenance activities are improved. In personnel training it is possible to transfer skills to operators in a reality like the actual one. The result is measured in operational efficiency, safety and customer service level." (generic, no data)

## Human participants; task time or error rate measured on people
**No human participants.** The word "participant" does not appear anywhere in the document. Times are desk estimates from a scoring table, described as such: "A system of scoring has been defined for the elementary operations that are part of the disassembly, taking as reference the simplest operation, that is to remove an easily graspable object without the exertion of a lot of manual force by a trained worker in average condition." (pp. 4 and 5). No task time or error rate was measured on people; the words "error rate" do not occur. The paper contains **no sentence** disclaiming such metrics.

## Authors' own limitations, difficulties, future work
- p. 29: "the differences of which are negligible due to the low complexity of the speed reducer, which has a Bill of Materials limited to a few types of components."
- p. 28: "This approach allowed to technically realize the sequences calculated by each methodology, that, for the matters of this study, was constrained to one operator, so the disassembly sequences were stated as 'sequential', instead of 'parallel'."
- p. 8, method rejected as unusable on this object: "later research by (Gulivindala, Bahubalendruni, Inkulu et al., 2021) suggested the Extended Part Concatenation Method to solve both sub-assembly detection to perform a linear assembly sequence planning. but the proposed method could not be adapted to the physical constraints of the speed reducer."
- p. 8, list of simplifying assumptions, including: "Part movements during assembly operations are ideal assuming no gravitational and friction forces are considered."; "Non-destructive disassembly or 'reverse assembly' approach."; "Matrices with no interference would be generated considering straight line part movements along six main directions on three axes."
- p. 24: "For simplicity its assumed to not having an adjustable wrench."
- p. 29, forward-looking statement: "It is also possible to think about showing, during the simulation, the tools to use and the simplest way to perform the operations."
- p. 29: "In addition, an additional calculating criterion for the optimal sequence yielded different results, as previously thought by the research team."
- On authoring effort of the AR content: **nothing stated.** The AR build is described procedurally (pp. 27–28) with no statement of time, cost or difficulty. There is one statement about method effort, not AR authoring effort (p. 17): the GA "proved to be a reasonably quick method to implement manually".
- No section headed Limitations, Future Work, or Conclusions; the paper ends with Section 4 "Discussion" (p. 29).

## Five verbatim headline sentences
1. "The methodology proposed compares three probable disassembly sequences following a comparison of literature-found procedures between genetic algorithms and as a 'state space search' problem, followed by a hybrid approach developed by the authors." (p. 2)
2. "AR technology in this study is a tool used to better understand the matters of each proposed sequence arrived from a theoretical foundation." (p. 2)
3. "AR would become the main tool that helps designers to technically validate the process that has been demonstrated thru the calculated sequences." (p. 6)
4. "The sequence with the lowest value was the customized sequence, with ET equal to 2.87 minutes, which differs from the other proposals by tenths or hundredths of a minute." (p. 28)
5. "As for disassembly, at this point it is intuitive to imagine a digital manual, with which the user is better able to familiarize himself with the assembly than a traditional paper manual." (p. 29)

## Internal inconsistencies
1. **TMU × unit value does not equal the printed ET, by a factor of 10 (Table 8, p. 28; Table 7 total, p. 23).** Printed: "Total time (TMU) 478.5 / Unit value (TMU) 0.036 / Estimated time (ET) (sec.) 172.26". 478.5 × 0.036 = 17.226, not 172.26. Same for the other two rows on p. 28: "480.7 … 0.036 … 173.052" (480.7 × 0.036 = 17.3052) and "485.0 … 0.036 … 174.60" (485.0 × 0.036 = 17.46). The minutes column is internally consistent with the seconds column (172.26 s = 2.871 min), so the break is between the TMU total × unit value and the ET.
2. **"Significant deviations" vs "negligible" in the same sentence pair (p. 29):** "This allowed to reach the best sequence for disassembly times, noting significant deviations. the differences of which are negligible due to the low complexity of the speed reducer". The reported spread is 2.871–2.910 min (p. 28), and pp. 27 and 28 both call it "tenths or hundredths of a minute".
3. **Figure numbering.** p. 9 refers to "Construction of a component-fastener graph (Figure 4)", but the caption of Figure 4 (p. 10) is "Speed reducer assembly rendering", and the component-fastener graph is Figure 8 (p. 16). p. 9 also refers to "the interference matrix (Table S2)", whereas the interference matrix is Table 2 (p. 16). p. 17 states "The model of this sequence is displayed before on Figure Figure 4." and p. 14 states "Figure 4 shows the rendering of the entire assembly except screws, upper casing and covers." Repeated typographic duplication "Figure Figure" occurs on pp. 12, 17, 20, 24, 26, 27.
4. **Table 5 title vs content (p. 19):** the table is titled "First Disassembly Frequency" while its columns are "Components that minimize f(.)", "Chosen Sequence" and "Evaluation function f(.)" and the text calls it the disassembly sequence ("the disassembly sequence is shown in Table 5", p. 17).
5. **Section-2.1.2 cross-reference (p. 17):** "the search was carried out according to a penalty function, that was the same one used for the previous sequence" — while p. 15 attributes the penalty-cost method for the *first* sequence to the same source used for the second: "A method for evaluating disassembly costs was followed after the study of Tseng et al. (Tseng et al., 2018)."
6. **Coverage of C12.** The PSO sequence (p. 28) contains 24 entries and omits C12 (Bottom carcass), consistent with p. 25 ("So only the lower casing and my disassembly operation ends were left"), but the GA sequence on the same page ends "…-C3-C1-C12", i.e. it does remove the bottom carcass. The three sequences are therefore not to the same disassembly depth, and the paper compares their total times directly (pp. 27, 28) without noting this.
7. **Reference-list metadata:** "Zhang, Y., Wang, S., & Ji, G. (2009). A Comprehensive Survey on Particle Swarm Optimization Algorithm and Its Applications. Mathematical Problems in Engineering, 2015(1), Article 931256." (p. 30) — dated 2009 in the author-year position and 2015 in the volume position.

---

# Paper 3 — `Mixed reality lab for assembly and disassembly of industrial products.pdf` (10 PDF pages)

## Citation as printed
- **Authors:** Simone Cantarelli, Daniela Francia, Gian Maria Santi, Alfredo Liverani, Matteo Fiori (p. 1). Affiliation: "Università di Bologna, Italia 40136, Italy" (p. 1)
- **Year:** 2025 ("Available online 15 May 2025"; "© 2025 The Authors", p. 1). Received/accepted/revised dates: **not printed** (the A R T I C L E I N F O block carries only Keywords).
- **Full title:** "Mixed reality lab for assembly and disassembly of industrial products" (p. 1)
- **Journal:** *Journal of Industrial Information Integration* (p. 1 masthead and running footer on every page)
- **Volume:** 46 (running footer: "Journal of Industrial Information Integration 46 (2025) 100873")
- **Issue:** not printed
- **Article number:** 100873
- **Page range:** no journal page range printed; running footers number the article pages 1–10.
- **DOI:** 10.1016/j.jii.2025.100873 (p. 1)
- Publisher/licence: "2452-414X/© 2025 The Authors. Published by Elsevier Inc. This is an open access article under the CC BY-NC-ND license" (p. 1). ISSN 2452-414X.

## Type of work, in the paper's own words
- Label above the title: "Full Length Article" (p. 1).
- p. 1 (abstract): "This paper presents a comprehensive study on the development and implementation of a Mixed Reality Lab designed for the assembly and disassembly of industrial products."
- p. 9: "This study presents the development and functional evaluation of a MR Lab designed to simulate industrial assembly and disassembly processes."
- p. 1: "The contributions of this work are threefold: 1. A workflow for efficient and accurate spatial digitization using laser scanning and mesh processing; 2. A VR environment enriched with dynamic interaction mechanisms tailored for mechanical assembly training; 3. A user-centred design approach that emphasizes procedural clarity and error mitigation in virtual tasks."
- Sections: "3. Project description and methodology" (p. 4), "4. Data analysis" (p. 8), "5. Conclusions" (p. 9).

## Object being disassembled, and how many parts/steps
- Primary case: a **Brake Assembly (BA)**, mounted on a vehicle mock-up. "The system is demonstrated using the case of a brake assembly, selected due to its structural complexity and relevance in training scenarios." (p. 1); "a Brake Assembly (BA) was selected, as it fulfils the essential criteria for validating the environment about handling multiple parts and constraints" (p. 7). Components originally in ".par (Solid Edge format)", converted to .obj (p. 7).
- **Table 1 (p. 7), "List of parts with quantity":** 9 codes — Whell rim 1, Disc 1, Locking sleeve 1, Hub 1, Caliper 1, Pin 4, Support 1, Hook 1, Wedge 1 (quantities sum to 12).
- **Table 2 (p. 7), "List parts and operations for assembly/disassembly":** Brake Assembly 12 parts / 12 operations; Tensioner 10 / 11; Drill Chuck 10 / 10; Ball Valve 30 / 32.
- Three additional assemblies tested (p. 7): "Tensioner, incorporating rotating elements and components necessitating screw-based fastenings."; "Drill chuck, incorporating parts that need both sliding motions and screw-based assembly."; "Ball valve, which is significantly more complex than the other three, with multiple components that necessitate screw fastening as well as insertion."
- Also scanned/reconstructed: the physical laboratory and an automotive mock-up (pp. 4–6).

## Where the disassembly sequence comes from, and where MR enters
- The order is fixed **before** the environment is tested: p. 4, §3.1.4: "The final phase of the project involved the definition of the assembly order, which must be established prior to the testing of the environment. In this phase, the assembly and disassembly order were established, with a focus on avoiding any potential interferences during the assembly process. Finally, the environment was tested, validating the previously inserted features and conducting a complete assembly and disassembly of the component."
- p. 7: "Following importation into the designated environment, a thorough examination of each component was conducted to establish a logical sequence for assembly."
- p. 7: "To determine the optimal assembly and disassembly order, methodologies derived from Assembly Sequence Planning (ASP) and Disassembly Sequence Planning (DSP) were employed. The ASP methodology involves the systematic evaluation of geometric constraints, functional dependencies and ergonomics in order to define a logical and efficient sequence of assembly. Concurrently, Digital Signal Processor (DSP) principles were applied to ensure ease of maintenance and the reversibility of assembly procedures, thus minimising complexity and reducing operational downtime."
- p. 7, the sequence as printed: "The sequence is as follows: Hub, Disc, Caliper, Pin (4), Hook, Wedge, Support, Wheel Rim, and Locking Sleeve." Disassembly is its reverse: "Subsequently, a complete disassembly was performed, thereby reversing the steps that had been previously executed." (p. 7); "The disassembly procedure, which is executed in reverse order to the assembly process, presented similar challenges." (p. 8)
- p. 3: "In the proposed system, the integration of ASP and DSP with MR is realised through a Unity-based environment that utilises 3D scanned meshes and precomputed interaction sequences. These sequences, derived from topological and functional analyses, are rendered through interactive overlays and dynamic part behaviours (e.g., snapping, colour-coded interlocks, and guided motion paths)."
- p. 7, criterion stated for the environment: "Condition 1 stipulates that the assembly sequence should not be predetermined; rather, an optimal sequence should be identified to facilitate part assembly."

**No named algorithm, solver, matrix, graph or objective function is given for this paper's own sequence.** ASP/DSP are described generically at pp. 2–3 (as literature concepts) and invoked as "methodologies derived from" at p. 7; the sequence itself is stated to come from "a thorough examination of each component" (p. 7).

**Explicit status:** the display did **not** plan the sequence. Unity/MR is the environment in which the already-defined order is executed, guided and stress-tested; the guidance mechanism is a colour script, not a planner — p. 7: "a concise C# script was devised to offer visual guidance during the assembly sequence, whereby components are distinguished by their colour as they progress through the assembly line, facilitating immediate identification by the operator." Validation is procedural/geometric: p. 9: "the evaluation focused on functional validation through visual inspection and procedural accuracy."

## Every quantitative result, with PDF page number
**Laboratory scan (FARO Focus X 130, p. 4):**
- "four scans were executed with the objective of eliminating shadow cones and other forms of interference" (p. 4)
- "360◦horizontal and 300◦vertical scans were conducted at high resolution (with a precision of 0.3 mm and a 170-megapixel colour HDR camera)" (p. 5)
- "The preliminary scans were completed in approximately 15 min each" (p. 5) — unit as printed: minutes; the paper states no conversion to seconds. (15 min = 900 s; that conversion is not stated by the paper.)
- "additional scans were conducted, specifically three" (p. 5)

**Mock-up scan (FreeScan UE Pro, Shining 3D, p. 5):**
- "The FreeScan UE Pro has a precision of 0.02 mm"
- "the vehicle divided into four distinct sections"; "only half of the model was scanned"
- "The mesh was decimated to make it suitable for use on a headset, with the resulting weight of 31 MB representing a significant reduction from the original 700 MB. Finally, the mesh was mirrored to create a complete model with a weight of 67 MB"

**Meshing (pp. 4, 6, 8):**
- "While Poisson reconstruction yielded better results, it was computationally intensive and resulted in a file size of 13 GB, which was impractical." (p. 4)
- "The final mesh, exported in .obj format, had a manageable size (approximately 6 GB)" (p. 4); "The resulting file, with dimensions of approximately 6 GB, could be imported into Unity without significant issues" (p. 6)
- "An intermediate reconstruction was created using a precision level of 8" (p. 6)
- "very low values were inserted as parameters for uniformity and element size, specifically 0.1 for Smoothing and 0.2 for Target Size" (p. 8)
- "A comparison of measurements between the scan and the mesh at sample points revealed that the precision of 1 mm was maintained throughout the scan, with the measurements taken from the mesh matching those taken from the original source. In particular, the width of the work table, which was determined to be 0.943 m based on FARO imaging, was found to be consistent with the value derived from the mesh." (p. 8)

**Scanning-technology figures cited from literature (p. 3):** structured light "with a margin of error of up to 0.01 mm"; LiDAR "can acquire points at rates in excess of megahertz".

**Software/hardware versions:** "Unity 2022.3.29f1" (p. 6); HTC Vive Pro then "Meta Quest 2 and Meta Quest 3" (p. 6).

**Parts and operations (Table 2, p. 7):** Brake Assembly 12 / 12; Tensioner 10 / 11; Drill Chuck 10 / 10; Ball Valve 30 / 32.

**Test cycles (Table 3, p. 9; text pp. 9, 9–10):**
- "The BA was subjected to a total of 25 test cycles, the drill chuck and the belt tensioner each underwent 15 test cycles, and the ball valve assembly underwent 10 test cycles." (p. 9)
- Table 3, "Number of tests divided by assemblies": Brake Assembly 25; Drill Chuck 15; Belt Tensioner 15; Ball Valve 10 (p. 9)
- "The evaluation phase involved a total of 65 test cycles on four mechanical assemblies: 25 for the brake assembly, 15 each for the drill chuck and belt tensioner, and 10 for the ball valve." (p. 9)

**Geometric error observed:** "partial penetrations of approximately 5 mm were consistently observed during these operations" (p. 9); repeated at p. 9: "partial mesh penetrations of approximately 5 mm during certain insertions, which did not affect overall task completion."

**No time saving, no task time, no error rate, and no percentage improvement is measured anywhere in this paper** — see next-but-one field.

## What the authors attribute the improvement to
The paper reports no measured improvement. Its stated key mechanism is the mesh/collider engineering and the scanning workflow, not a display effect:
- p. 9: "The key innovation of this work is the integration of laser-based spatial capture with adaptive guidance through dynamic collider activation, providing a responsive and intuitive training environment. The hybrid mesh strategy also enabled efficient reconstruction of complex laboratory spaces without compromising spatial accuracy."
- p. 9: "consistent management of mesh penetration and accurate part alignment across all scenarios provided strong qualitative evidence of system reliability."
- p. 9: "The successful resolution of these geometric conflicts through the dynamic activation and deactivation of mesh colliders confirmed the system's capability to support coherent and collision-free manipulation of components. This qualitative validation demonstrates the reliability of the simulation environment for training and procedural verification purposes."
- p. 8: "Consequently, the result was a particularly precise mesh, with its accuracy further enhanced by the decision to make it hybrid."

Unsupported summary claim in the abstract (no measurement behind it in the paper): p. 1: "The results indicate significant improvements in training efficiency and operational precision, with potential applications across various industrial sectors."

Claims about MR/VR benefit in general are attributed to cited literature, not to this study (pp. 2–3), e.g. p. 2: "The primary advantage of MR is its ability to minimise human error, expedite training, and enhance real-time situational awareness during high-complexity operations." (in §2.2, State of the art).

## Human participants; task time or error rate measured on people
No participant study, no participant count, no demographics. Human involvement mentioned only as the authors'/experts' own use:
- p. 6: "some tests were carried out to evaluate the functionality of the imported features, such as simultaneous user access, voice chat and, crucially, interactions that users could have with simple sample objects."
- p. 8: "underwent thorough testing during initial implementation and was iteratively refined based on feedback."
- p. 9, KPI list includes "User Experience and Immersion: Qualitative assessment derived from user observations and feedback."
- p. 9: "the testing phase relied on functional and procedural validation through stress-testing, iterative trials, and expert visual inspection."

**The paper states explicitly, twice, that no such metrics were collected:**
- p. 9: "While a framework of standardised performance metrics was outlined to contextualise the MR system within established evaluation practices, it must be emphasised that no formal quantitative data collection (e.g., completion time, error rates) was performed."
- p. 9: "It is worth noting that no quantitative performance metrics such as task completion time or error rate were collected during the testing phase. However, the evaluation focused on functional validation through visual inspection and procedural accuracy."
- p. 9 (Conclusions): "Although no quantitative performance metrics such as task completion time or error rates were collected, consistent management of mesh penetration and accurate part alignment across all scenarios provided strong qualitative evidence of system reliability."
- p. 10, Data availability: "No data was used for the research described in the article."

## Authors' own limitations, difficulties, future work, and authoring effort
Authoring / content-creation effort and pipeline difficulty:
- p. 4: "The initial attempt to process this data using MeshLab to reconstruct the mesh was unsuccessful. Consequently, the decision was made to use CloudCompare, which offers a variety of reconstruction methods. Initially, Delaunay Triangulation was employed; however, the results were found to be imprecise."
- p. 4: "Subsequent attempts to generate an acceptable mesh with CloudCompare using Delaunay Triangulation … and Poisson Surface Reconstruction … were unsuccessful. Delaunay Triangulation was unsuccessful in generating a mesh of the desired quality. While Poisson reconstruction yielded better results, it was computationally intensive and resulted in a file size of 13 GB, which was impractical."
- p. 6: "To address this issue, it was necessary to eliminate non-essential parts that were particularly dense in points, such as desktop computers, 3D printers, and other objects that were of limited use. … Despite the removal of objects led to the creation of gaps in areas originally covered, the desired reduction in size was successfully achieved".
- p. 5: "the colour representation was lost, resulting in a grayscale representation. Secondly, the point cloud exhibited numerous points caused by shadows and background noise interference, which appeared somewhat 'dirty'."
- p. 5: "it was observed that some parts of the laboratory were not covered by the scans, resulting in areas devoid of points."
- p. 6: "during this phase, challenges arose with reference to the capabilities of the HTC Vive Pro headset: it had significant difficulties in providing a smooth view of the new space, leading to a change in equipment."
- p. 1 (framing the general problem): "Existing technologies such as laser scanning and LiDAR have been widely applied in 3D reconstruction, but they often involve time-consuming post-processing workflows and may lack direct integration with interactive simulation platforms."

Technical limitations of the result:
- p. 8: "The initial approach, which employed layers to regulate object interactions, was found to be impractical due to its complexity and the inability to dynamically manage multiple interactions simultaneously."
- pp. 8–9: "the system frequently exhibited partial penetrations and sporadic collisions between components during linear movements towards user interaction points. While these issues did not have a significant impact on the overall operational success, they did have a minor effect on the realism and intuitiveness of the interactions".
- p. 9: "due to limitations in the software preventing accurate elastic material simulation, partial penetrations of approximately 5 mm were consistently observed during these operations."
- p. 9: "Minor limitations were observed in the simulation of elastic deformation".
- p. 8: "The only imperfections noted were related to approximate point removal in certain parts and some inaccuracies in the scan".
- p. 1: "While the present work does not aspire to replicate the totality of the functions of a Digital Twin, it establishes a foundational virtual environment that has the potential to support future integration with live sensor data or predictive analytics modules."

Future work (p. 10):
- "Future research will explore the integration of real-time sensor data for improved synchronisation with physical systems to support the development of digital twins. Additional efforts will be devoted to improving elastic behaviour simulation and introducing multimodal feedback, including haptics, to further enhance training realism and operator engagement."
- Also p. 2: "The final section (Section 5) outlines the conclusions and future directions."

## Five verbatim headline sentences
1. "The primary objective is to create an advanced and intuitive Virtual Reality (VR) environment that facilitates industrial operators' learning and reduces training time while minimising errors." (p. 1)
2. "The results indicate significant improvements in training efficiency and operational precision, with potential applications across various industrial sectors." (p. 1)
3. "To determine the optimal assembly and disassembly order, methodologies derived from Assembly Sequence Planning (ASP) and Disassembly Sequence Planning (DSP) were employed." (p. 7)
4. "It is worth noting that no quantitative performance metrics such as task completion time or error rate were collected during the testing phase." (p. 9)
5. "The key innovation of this work is the integration of laser-based spatial capture with adaptive guidance through dynamic collider activation, providing a responsive and intuitive training environment." (p. 9)

## Internal inconsistencies
1. **Claimed improvements vs stated absence of measurement.** Abstract, p. 1: "The results indicate significant improvements in training efficiency and operational precision, with potential applications across various industrial sectors." Against p. 9: "it must be emphasised that no formal quantitative data collection (e.g., completion time, error rates) was performed." and p. 9: "It is worth noting that no quantitative performance metrics such as task completion time or error rate were collected during the testing phase." and p. 9: "Although no quantitative performance metrics such as task completion time or error rates were collected…".
2. **DSP acronym expanded two different ways.** p. 2: "The processes of Disassembly Sequence Planning (DSP) and Assembly Sequence Planning (ASP) are of particular interest in this study." Against p. 7: "Concurrently, Digital Signal Processor (DSP) principles were applied to ensure ease of maintenance and the reversibility of assembly procedures".
3. **Poisson reconstruction rejected, then used.** p. 4: "Subsequent attempts to generate an acceptable mesh with CloudCompare using Delaunay Triangulation … and Poisson Surface Reconstruction … were unsuccessful. … While Poisson reconstruction yielded better results, it was computationally intensive and resulted in a file size of 13 GB, which was impractical. Consequently, Houdini was employed to generate the final mesh." Against p. 6: "Secondly, the point cloud was exported in .pts format and processed using CloudCompare. Meshing and surface reconstruction were then performed using the Poisson method. An intermediate reconstruction was created using a precision level of 8, which allowed for the maintenance of intermediate quality while keeping computation times low."
4. **Number of scans merged.** p. 4: "four scans were executed"; p. 5: "additional scans were conducted, specifically three" (seven total). Against the caption of Fig. 6, p. 5: "Raw point cloud after merging the three scans."
5. **VR vs MR labelling of the same system.** Title and Conclusions: "Mixed reality lab for assembly and disassembly of industrial products" (p. 1); "a MR Lab designed to simulate industrial assembly and disassembly processes" (p. 9). Against the abstract, p. 1: "The primary objective is to create an advanced and intuitive Virtual Reality (VR) environment"; §3 heading text, p. 4: "This will be achieved through the development of an application in the field of VR"; p. 9 contributions: "A VR interaction system with dynamic collider control". Also p. 2 defines the two as distinct: "In contradistinction to VR, which engenders a sense of total isolation within a completely simulated environment, MR sustains spatial awareness and enables real-time co-presence with physical equipment."
6. **KPI list vs data collection.** p. 9 states "the following key performance indicators were monitored: • Task Completion Time: Average duration of full assembly and disassembly procedures; • Accuracy Rate: Proportion of correctly positioned components without collisions or misalignments; • Collision and Penetration Events: Frequency and severity of mesh-related errors; …" — no values for any of these are reported, and the next paragraph on the same page states none were collected.
7. **Figure cross-reference.** p. 6: "the next step was to import the necessary components in order to perform the final test of the room's functionality (Figs. 3)." Figure 3's caption (p. 5) is "First point cloud with errors." Similarly, p. 5's §3.2.2 (mock-up mesh) is followed by Fig. 5, captioned "Screen of the mesh obtained in Unity", while the text of §3.2.2 ends "which can be rendered in Unity (Fig. 5)" — consistent — but §3.2.3 (laboratory, ~6 GB) ends "could be imported into Unity without significant issues (Fig. 6)" and Fig. 6's caption is "Raw point cloud after merging the three scans."
8. **Table 1 spelling:** code 1 is printed as "Whell rim" in Table 1 (p. 7) and as "Wheel Rim" in the sequence text on the same page.agentId: a1e57ec315d723f63 (use SendMessage with to: 'a1e57ec315d723f63', summary: '<5-10 word recap>' to continue this agent)
<usage>subagent_tokens: 172603
tool_uses: 25
duration_ms: 531666</usage>
---

# PART 6 — Kaarlela, Yang, and the three unidentified files

## 1. `Digital_twin_and_extended_reality_for_teleoperatio.pdf`

**Citation as printed** — Tero Kaarlela, Sami Salo, Jose Outeiro, "Digital twin and extended reality for teleoperation of the electric vehicle battery disassembly." Journal/conference: **not stated**. Volume/issue/page range: **not stated**. Bottom of p. 1 carries the stamp `arXiv:2507.14929v1 [cs.RO] 20 Jul 2025`. PDF metadata: title and the three authors; creator "arXiv GenPDF (tex2pdf:)"; no DOI field. **This is an arXiv pre-print, not a peer-reviewed publication.** No DOI printed.

**Type of work, in the paper's own words** — "This publication presents a concept of DT-based teleoperation for disassembly and sorting EVBs" (p. 2); "The outcome of the presented work is the concept of a teleoperation system" (p. 4).

**What was studied** — Teleoperated pack-to-module disassembly/sorting of a PHEV battery ("nominal capacity of the EVB is 12.5 kWh… nominal voltage is 400 volts… 1.5 x 0.8 x 0.5 meters, and weight is 174 kgs", p. 2) with a Kuka KR10 R1100 on a linear track, ROS digital twin, Unity XR interface, MegaPose 6DoF pose estimation, RGB camera. Participants: "Thirty volunteers piloted the system and provided feedback" (p. 5), recruited via a social-media link; background self-reported only via one added question on robotics expertise; no allocation to groups; conditions compared: manual vs robotized disassembly times (Table II, p. 4). Hardware-in-the-loop was not run: "software-in-the-loop support was implemented to test the concept at the software level only" (p. 6).

**Every quantitative result**
- p. 2: EVB specs 12.5 kWh, 400 V, 1.5×0.8×0.5 m, 174 kg; workspace "seven degrees of freedom and a working area of 4 x 2 meters".
- p. 4, Table II (manual vs robotized, seconds): cover screw removal 645 / 75; battery cover removal 6 / 12; wiring connectors detach 71 / 240; battery module screws removal 76 / 92; printed "Total: 258 sec / 474 sec".
- p. 4, Table III (costs, €): robot and linear track 39 500; steel frame 2600; automatic tool changer 2808; vacuum gripper 1140; wiring connector detach gripper 2300; EVB reverse design 1100; digital twin creation 8300; user interface programming 5600; printed "Total: 108 256 €".
- p. 4: "spent 32 working hours reverse engineering the PHEV battery".
- p. 5, Eq. 1: "ROI = NS −IC / IC = 100000 € −108256 € / 108256 € = −7.63%".
- p. 5, Table IV: full five-point Likert distributions for the ten SUS items plus the added item ("I feel that I am an expert in robotics": 50.0% / 16.0% / 17.0% / 0.0% / 17.0%).
- p. 5: "a mean SUS score of 65.4"; "about 35% feel the system is cumbersome".
- p. 6: "only 40% felt confident using the system".
No inferential statistical tests are reported.

**How the system decides what to show the user** — "The CAD model of the EVB includes all the components of the assembly… Each component includes a tag that defines the strategy, the orientation of the approach, and the tool necessary for detaching. For example, if the teleoperator clicks on a horizontally mounted screw component of the EVB, unscrew tool is activated and positioned at the approach point" (p. 3). "The position and orientation of the EVB are updated in real time using the MegaPose and an RGB camera. Based on the CAD model and video input, the MegaPose library identifies the location and orientation of the EVB" (p. 4). "The Robot Visualization (RVIZ) ROS package generates collision-free trajectories to reach the requested positions" (p. 4). "the DT guides the teleoperator by providing visual aids to generate collision-free trajectories" (p. 2).

**What the authors say is missing from the field or from practice** — "The diversity in the physical qualities of different battery types and the lack of design for disassembly require cognitive capabilities and dexterity comparable to those of humans" (p. 1); "Achieving fully autonomous EVB disassembly is challenging due to design variability, lack of standardization, and permanent mechanical joints" (p. 3); "While using CAD models in simulations offers geometric reference, simulations lack real-time updates and interaction capabilities" (p. 3); "The challenges are the higher upfront investments compared to manual disassembly, the limited availability of detailed CAD models, and the slower speed of the robot for tasks requiring dexterity" (p. 4).

**Authors' own stated limitations and future work** — "The feasibility of the presented approach relies on the accuracy of CAD models and of the pose estimation through MegaPose. Unfortunately, currently the KUKA ROS package [15] does not support controlling additional axes, which prevented hardware-in-the-loop experiments to validate this accuracy. Therefore, software-in-the-loop support was implemented to test the concept at the software level only" (p. 6). "Future work will further automate decision-making processes, improve real-time adaptability and user interface, controls, and conduct hardware-in-the-loop experiments to validate the proposed approach in an industrial setting" (p. 6). Also "The first question I think that would like to use this system frequently in this study is misleading" (p. 5); "Future research can further automate perception, decision-making, and recovery processes to reduce human intervention" (p. 4).

**Five verbatim headline claims**
1. "We propose a teleoperated system for the safe disassembly and sorting of EVBs." (p. 1)
2. "This hybrid approach combines teleoperation and automation to improve safety, adaptability, and efficiency in EVB disassembly and sorting." (p. 1)
3. "The economic contribution is realized by reducing labor dependency and increasing throughput in battery recycling." (p. 1)
4. "The analysis of SUS scores shows a mean SUS score of 65.4, which is considered a good value." (p. 5)
5. "While the initial ROI is negative due to upfront investment, labor dependency cost reductions and safety improvements are expected to offset this within approximately one year of operation." (p. 5)

**Internal inconsistencies**
- Table II column sums do not equal the printed totals (p. 4): manual 645+6+71+76 = 798 sec against printed "Total: 258 sec"; robotized 75+12+240+92 = 419 sec against printed "Total: 474 sec".
- Table III item prices sum to 63 348 € against printed "Total: 108 256 €" (p. 4).
- Efficiency/throughput claim vs measured times: "improve safety, adaptability, and efficiency in EVB disassembly and sorting" and "increasing throughput in battery recycling" (p. 1) against Table II robotized total (474 sec) exceeding manual total (258 sec) (p. 4).
- Robotics expertise: "The added question reveals that only half of the participants consider themselves as experts in robotics" (p. 6) against Table IV, where the item "I feel that I am an expert in robotics" shows "Strongly disagree 50.0%… Agree 0.0%, Strongly agree 17.0%" (p. 5).
- Participant count vs response denominators: "Thirty volunteers piloted the system" (p. 5) against Table IV percentages implying differing denominators across items (3.1% and 40.6% imply n=32; 58.1% and 9.7% imply n=31; 33.3% and 20.0% imply n=30) (p. 5).
- "The investment becomes profitable after the first year of operation by reducing labor dependency" (p. 5) immediately preceding "ROI = … = −7.63%" (p. 5).

---

## 2. `X-reality and lean in end-of-life aircraft disassembly planning, critical review · PMC.pdf`

**Citation as printed** — Yinong Yang, Samira Keivanpour, Daniel Imbeau, "Integrating X-reality and lean into end-of-life aircraft parts disassembly sequence planning: a critical review and research agenda." *The International Journal of Advanced Manufacturing Technology*. DOI `https://doi.org/10.1007/s00170-023-11581-0` (p. 1). Volume/issue/page range: **not stated** (online-first). "Received: 5 February 2023 / Accepted: 10 May 2023"; "© The Author(s), under exclusive licence to Springer-Verlag London Ltd." (p. 1). Article-type banner: "CRITICAL REVIEW". PDF metadata confirms title, author "Yinong Yang", subject line with the DOI. Peer-reviewed journal article; **not a pre-print**; no arXiv stamp.

**Type of work, in the paper's own words** — "CRITICAL REVIEW" (p. 1); "This review is intended to be inductive and comprehensive to answer RQs" (p. 9); "This paper employs a structured literature review to combine two enablers, XR and lean" (p. 25); "there is no systematic review to address DSP, XR, and lean for EoL aircraft treatment. The objective of the present article is to fill this gap" (p. 3).

**What was studied** — Literature on disassembly sequence planning (DSP) for end-of-life aircraft parts, Industry 4.0, X-reality (VR/AR/MR) and lean; databases Web of Science Core Collection and Elsevier ScienceDirect; bibliometric mapping with "VOSviewer (version 1.6.18)" (p. 14); no device, task or system of the authors' own. **There were no participants**; no experiment. Conditions compared: two search "views" (initial view in WoS; network view in SD) and two search steps (Table 5, p. 13).

**Every quantitative result**
- p. 1: "85% of the aircraft (Airbus A300) content was recycled in 2005"; "90% of the used serviceable content of soon-to-be-retired aircraft could be recycled or reused".
- p. 2: "the extra serviceable time they can extend was estimated to be at least 10 years"; "approximately 13,000 planes would be scrapped by 2030, while Flightglobal estimated this figure to be 17,000"; "There are eight types of waste".
- p. 10: "a total of 1655 journal articles were listed"; time frame "2011 and 2023".
- p. 11: journal shares — "International Journal of Production Research (18.369%), International Journal of Production Economics (13.474%), Production Planning Control (12.689%), and European Journal of Operational Research (4.894%)"; "Expert Systems with Applications, Journal of Manufacturing Systems, International Journal of Computer Integrated Manufacturing, and Annals of Operations Research contained 3.807%, 3.625%, 3.384%, and 3.021%".
- p. 13, Table 5 retrieval counts: 15, 32, 353, 114 (first step); 56, 64, 27 (second step); text repeats "353 results" and "only 56".
- p. 14: "A total of 194 relevant publications were retrieved"; "removed an additional 19 results. Therefore, 175 final samples"; "the occurrence numbers of the overall keywords' frequencies are set as five, and ultimately, there are 24 highlighted keywords (nodes)"; "five clusters formed in five different colors".
- p. 22: "the relevant term 'smart disassembly' only occurred once in [121]".
No statistical tests are reported.

**How the system decides what to show the user** — The paper describes reviewed systems, not one of its own: "[92] recommended… that the mathematical modeling of an EoL aircraft and its subsystems could utilize the Aircraft Maintenance Manual (AMM) as the only data source" (p. 16); "the information sources introduced by [95] comprised three categories: (1) primary sources: any data sheets or illustrations; (2) expert data… and (3) machine data: the data obtained from visual techniques" (p. 16); "The middle step and the significant part of the ARDIS was 'automatic content generation,' which was the transition from visual information (e.g., 3D models) and optimal sequence results to the AR guideline application" (p. 16); "the virtual objects are animations, images, or text from a perspective determined by the camera's direction and position, and AR allows real-time blending through the developed computational design and program" (p. 16); "the effects of XR instructions and guidelines rely on the design and development of user interfaces. For instance, optimal action plans can be displayed in XR applications that authored by SDSP" (p. 24).

**What the authors say is missing from the field or from practice** — "XR has a broad application prospect, but collaborative XR applications are still missing in EoL DSP" (p. 2); "the XR–lean collaboration remains unstudied" (p. 2); "lean improvements that could be attained through XR adoption in DSP are still in the nascent stages" (p. 3); "the scholastic content of real-life I4.0 applications' evaluation and examination is minimal" (p. 7); "The evolution of decision-making methods toward a smarter DSP is neglected in the majority of the extant literature, and the cooperation and functions of I4.0 technologies are rarely involved in the disassembly process" (p. 7); "While industrial XR has a broad application prospect, its real-life deployment for EoL aircraft parts is still a challenge because its practical exploration remains insufficient" (p. 8); "remanufacturers still lack a comprehensive understanding of how I4.0 technologies can affect the established lean disassembly" (p. 8); "the collaboration between computational intelligence and scientific management is nearly absent" (p. 16); "The practicability and intelligence of AR technology have not been explored deeply and exclusively for the EoL information modeling and sequence optimization phases" (p. 18); "A survey of the best VR practices for EoL aircraft treatment is also lacking" (p. 17); "only several software tools can convert between CAD models and XR-ready models directly, but research efforts on bi-directional linkages and information exchange standards are still limited" (p. 24); "experiment-based empirical studies are still in the early stages of XR-assisted disassembly development" (p. 24).

**Authors' own stated limitations and future work** — "While the basic descriptive analysis of the network visualization is limited…" (p. 15); "Other I4.0 technologies are temporarily disregarded due to the original intention of concrete contribution from applying XR individually" (p. 25); "the present study does not review the impact of lean on environmental potentials and drawbacks" (p. 8); Table 12 lists the agenda items 1A–3B (p. 23), including "2F. Carry out more research through expert interviews…" and "3B. Expand the research focus to encompass the emerging wave of Industry 5.0"; "the testing and validation from case studies and experimental studies should be implemented in the decision-support system to verify the XR–lean method" (p. 26); "As XR devices are wearable, the ergonomics analysis of human–XR collaboration cannot be lacking" (p. 26); "future research can include the measurement and analysis of the efficiency and social sustainability impacts of EoL aircraft parts' cases" (p. 26).

**Five verbatim headline claims**
1. "Since the research of integrating lean and XR into the EoL DSP is underexplored in literature, XR and lean are investigated as assistive enablers in the DSP." (p. 1)
2. "To the best of our knowledge, there is no systematic review to address DSP, XR, and lean for EoL aircraft treatment." (p. 3)
3. "SDSP refers to a productivity-enhancing disassembly decision-support construct that optimizes operative manners with the strong assistance of XR and lean." (p. 22)
4. "From the existing research findings, it can be concluded that XR–lean integration has the potential to improve the digitalization, quality, and effectiveness of EoL DSP." (p. 25)
5. "Notably, the collaboration between computational intelligence and scientific management is nearly absent, and the same can be said for radical improvement in flexibility and efficiency." (p. 25)

**Internal inconsistencies**
- Database attribution: "In this stage, the search engine SD was used to find the final revision documents" (p. 12) and "The number of retrieved publications in the SD engine was only 56" (p. 13) against Table 5's column header "Publications retrieved in Scopus" (p. 13).
- Source attribution for Table 10: caption reads "The potential correlation between 'Lean 4.0' and XR (the source of information: [111])" (p. 21) while the body text attributes the same content to [110]: "[110] defined the term 'Lean 4.0'… In particular, VSM 4.0, VM 4.0, and Poka-yoke 4.0 were mentioned" (p. 20).
- Reference [94]: Table 7 lists "[94] Eight different disassembly/dismantling strategies under the project 'Process for advanced management and technologies of aircraft end-of-life'" (p. 16) while the body says "[94] focused on the disassemblability index assessment, even though it was involved in the design phase" (p. 17); the "eight disassembly/dismantling strategies" are later attributed to [121]: "They presented 'smart disassembly' as one of their eight disassembly/dismantling strategies" (p. 22).
- Cross-reference numbering: "With regard to the search strings in Section 2.2.2" (p. 14) where the search strings are in Section 3.2.1/3.2.2 (pp. 12–13); "see Section 2.1.2.3" (p. 16) and "the previously discussed challenges in Section 5.2.2.1" / "Section 5.2.2.2" (p. 25) refer to section numbers that do not appear as headings.
- Review cut-off: "this review was finalized in October 2022… materials published up to 2023 were chosen" and "even though 2022 is not yet over" (p. 10) against "Received: 5 February 2023 / Accepted: 10 May 2023" (p. 1).

---

## 3. `AR Gestures.pdf`

**Identification** — Title: "Gesture-based Interaction for AR Systems: A Short Review". Authors: Despoina Gavgiotaki, Stavroula Ntoa, George Margetis, Konstantinos C. Apostolakis, Constantine Stephanidis (all Institute of Computer Science, FORTH-ICS, Heraklion, Crete, Greece; Stephanidis also Dept. of Computer Science, University of Crete). Year: 2023. Venue: PETRA '23 — Proceedings of the 16th International Conference on PErvasive Technologies Related to Assistive Environments, July 05–07, 2023, Corfu, Greece; ACM, New York, NY, USA. DOI: `https://doi.org/10.1145/3594806.3594815`. **One sentence:** It is a PRISMA-informed systematic review of 73 papers on hand-gesture interaction in AR systems, organised by context of use, that proposes a taxonomy of AR gestures and derives design findings.

**Files 3 and 4 are two different papers** — file 3 is this PETRA '23 AR-gesture review; file 4 is a CHI EA '23 empirical VR selection-technique paper (details in section 4).

**Citation as printed** — Gavgiotaki, Ntoa, Margetis, Apostolakis, Stephanidis (2023), "Gesture-based Interaction for AR Systems: A Short Review", in *Proceedings of the 16th International Conference on PErvasive Technologies Related to Assistive Environments (PETRA '23)*, July 05–07, 2023, Corfu, Greece, ACM, New York, NY, USA, "9 pages"; page footers run 284–292. Volume/issue: not applicable/not stated. "ACM ISBN 979-8-4007-0069-9/23/07"; DOI `10.1145/3594806.3594815` (p. 1). Peer-reviewed conference paper, **not a pre-print**; no arXiv stamp. PDF metadata: title as above, author field empty, creator "LaTeX with acmart".

**Type of work, in the paper's own words** — "In this state-of-the-art review, we classify the recent literature on hand gesture-based AR interaction techniques" (p. 1); "we have conducted a systematic literature review" (p. 1); "This review and meta-analysis were informed by the PRISMA guidelines [48], also applying the snowballing technique [76]" (p. 2).

**What was studied** — Literature on AR applications integrating hand-gesture interaction, classified by AR device, application domain and gesture type (Table 2, p. 4). Databases: "Science Direct, Scopus, ACM Digital Library, and IEEE Explore"; query "(augmented reality OR AR OR MR OR mixed reality) AND (gesture OR gesture-based interaction OR hand gesture OR mid air)"; "The search was not restricted to a specific time period" (p. 2). **There were no participants**; no experiment and no conditions compared by these authors.

**Every quantitative result**
- p. 2: "a corpus of 73 papers elicited from a pool of 412 papers that were initially retrieved"; "The search resulted in 413 papers"; "After removing duplicates a set of 369 papers remained for screening"; "the exclusion of an additional of 227 papers, leaving 142 papers for the eligibility checking phase"; "After this phase, 50 papers remained"; snowballing "resulting in the addition of 23 papers, and a total of 73 papers".
- p. 3, Table 1 (publications per domain): Virtual Objects Interaction 10; Medicine 10; Manufacturing 10; Education 9; Remote Collaboration 8; Cultural Heritage 8; Entertainment 7; UI Interaction 6; Smart Environments 5.
- p. 3: "A framework supporting twelve gestures each one tied with specific commands in an educational context"; "applied in the implementation of four distinct interactive systems".
- p. 5: "Three different games were developed for Google Glasses, where the users can interact through eleven hand and foot gestures"; "A study on AR guessability [55] involved individuals who were asked to elicit 800 gestures for 40 different tasks".
- p. 5: "based on 73 papers carefully selected from an initial set of 412 papers"; "engendering nine main categories".
- p. 4, Table 2: per-paper device/domain/gesture classification for 73 entries.
No statistical tests are reported.

**How the system decides what to show the user** — The paper describes no system of its own. The closest statements concern gesture recognition inputs in reviewed systems: "These studies also involve different approaches for gesture recognition such as Microsoft Kinect [9], [20], Leap Motion sensor [71] and stereo-vision cameras (OVRVision Pro VR stereo cameras) and an electromyographic signal monitoring bracelet [50]" (pp. 2–3); "For recognition of hand gestures there are reported, among others, image-based methods [61] and deep learning-based object identification and instance segmentation [51]" (p. 3); "The produced taxonomy also incorporates the technique used for the recognition of the gestures, namely through cameras (e.g., RGB cameras) or sensors (e.g., Microsoft Kinect, LeapMotion), as well as by hand or finger tracking depending on the type of the gestures" (pp. 5–6); "the base of all these studies is that the workers transmit through the HMD a live video of their real world surroundings and the remote helper assists them by capturing their gestures through tablets [60], HMD cameras [67], or large displays [81], [66]" (p. 3).

**What the authors say is missing from the field or from practice** — "At the same time, literature reviews on the topic are limited [28], [70] or they focus on a specific application domain [10]" (p. 1); "A major research question regarding this looming field… is which gestures to employ when designing and developing and AR system and how" (p. 1); "Considering the large amount of approaches available in literature, further insights are needed regarding which approaches are more suitable to adopt" (p. 5); "To outweigh the uncertainty regarding the mid-air gestures vocabulary supported by each system, a learning period is necessary" (p. 3).

**Authors' own stated limitations and future work** — No section is headed limitations. The forward-looking statements are: "we believe that the outcomes of this report and analysis will contribute to the informed design of gesture-based AR interactive environments and will initiate additional research and discussions in the field" (p. 5); and, reported from a cited study, "advised designers to… conduct follow-up studies comparing basic gestures, designer-refined gestures, menu options, and other modalities in multimodal interface designs" (p. 5). Own stated limitations: **not stated**.

**Five verbatim headline claims**
1. "Gesture-based interaction constitutes the prominent interaction modality for Augmented Reality (AR) systems." (p. 1)
2. "Furthermore, the discussed analysis reveals that context of use is a factor to take into account when determining the gestures that should be employed when developing an AR application." (p. 1)
3. "At the same time, there is a trade-off between touch-based and mid-air gestures, regarding their simplicity and user engagement entailed, which needs to be carefully considered." (p. 1)
4. "Studies of the aforementioned systems have shown that hand gesture interaction can improve task completion time and reduce operators' cognitive load." (p. 5)
5. "In all cases, a human-centered design approach should be followed to decide upon the final gestures' set." (p. 7)

**Internal inconsistencies**
- Initial pool size: "a corpus of 73 papers elicited from a pool of 412 papers that were initially retrieved" (p. 2) and "based on 73 papers carefully selected from an initial set of 412 papers" (p. 5) against "The search was not restricted to a specific time period. The search resulted in 413 papers" (p. 2).
- Table 2 device column contains a domain value: "Kim et al. 2017 | HMD (Remote Collaboration) | Remote Collaboration | Hololens supported gestures, Dynamic" (p. 4).
- Table 2 device/gesture mismatch: "Blankemeyer et al. 2018 | HMD (Microsoft HoloLens) | Manufacturing | Hololens 2 supported gestures" (p. 4).
- Statements on user preference: "Users exhibit a preference for traditional interactions (e.g., point and click or touch)" (p. 6) against "The majority of the hand gestures that were tested, elicited or integrated in the literature are dynamic gestures" and "Dynamic gestures are similar to the ways people interact with the real world around them… thus providing a more natural way of interaction" (p. 7).

---

## 4. `AR Gestures_tekinik.pdf`

**Identification** — Title: "Exploration of Bare-Hand Mid-Air Pointing Selection Techniques for Dense Virtual Reality Environments". Authors: Rongkai Shi, Jialin Zhang (both marked "Both authors contributed equally"), Yong Yue, Lingyun Yu, Hai-Ning Liang (corresponding) — all Xi'an Jiaotong-Liverpool University, Suzhou, China. Year: 2023. Venue: CHI EA '23 — Extended Abstracts of the 2023 CHI Conference on Human Factors in Computing Systems, April 23–28, 2023, Hamburg, Germany; ACM, New York, NY, USA. DOI: `https://doi.org/10.1145/3544549.3585615`. **One sentence:** It proposes two bare-hand mid-air pointing selection techniques (HandDepthCursor, HandConeGrid), re-implements MultiFingerBubble, and compares all three in an 18-participant VR target-selection study at two object densities. Despite the local filename, the paper is about **VR** target selection, not AR gestures.

**Citation as printed** — Shi, Zhang, Yue, Yu, Liang (2023), "Exploration of Bare-Hand Mid-Air Pointing Selection Techniques for Dense Virtual Reality Environments", in *Extended Abstracts of the 2023 CHI Conference on Human Factors in Computing Systems (CHI EA '23)*, April 23–28, 2023, Hamburg, Germany, ACM, New York, NY, USA, "7 pages". Volume/issue/page range or article number: **not stated**. "ACM ISBN 978-1-4503-9422-2/23/04"; DOI `10.1145/3544549.3585615` (pp. 1–2). Peer-reviewed conference extended abstract, **not a pre-print**; no arXiv stamp. PDF metadata: title, author, subject and keywords fields all empty; creator "LaTeX with hyperref".

**Type of work, in the paper's own words** — "we conduct a user study to compare and evaluate their performance and experience in a target selection task in dense VR environments" (p. 1); "The main contribution of this work is an empirical evaluation of three bare-hand mid-air pointing techniques for dense environments on VR HMDs" (p. 2); "As a first exploration of freehand pointing techniques for dense environments in VR HMDs" (p. 6).

**What was studied** — Task: "target acquisition task from [26]. Several spheres (radius = 10cm) were distributed in a 1m×1m×1m cuboid space 0.8m in front of the participants… one orange target sphere, and the remaining were white distractors" (p. 4). Device: "A Meta Quest 2… 1832×1920 per-eye resolution, an 89° horizontal field of view, and a 120Hz refresh rate… inside-out cameras allow 6 DoF hand tracking"; desktop "Intel i7-8700K CPU @ 3.70GHz… NVIDIA GeForce GTX 1080 Ti GPU, and 16GB of RAM"; "Unity (version 2021.3.36f1c1) with Oculus Integration SDK (version 42.0)" (p. 4). Participants: "We recruited 18 participants (7 females, 11 males) aged between 20 to 31 (M= 23.28, SD= 2.65)… all participants were right-handed. Six of them had normal or corrected-to-normal vision. Twelve were familiar or very familiar with VR HMDs… Five reported being familiar or very familiar with mid-air gesture-based input" (pp. 3–4); background not otherwise stated. Allocation: "We used a 3×2 within-subjects design with Technique (HDC vs. HCG vs. MFB) and Density (high vs. low)… We applied a Latin-square design to counterbalance the order of Technique conditions and randomized the order of Density in each Technique condition" (p. 4). Conditions compared: HandDepthCursor vs HandConeGrid vs MultiFingerBubble, at minimum inter-sphere distances of 20 cm (high density) and 30 cm (low density).

**Every quantitative result**
- p. 4: "In total, we collected 2160 trials of data (= 18 participants × 3 techniques × 2 levels of density × 20 repetitions)"; "minimum distances of 20cm and 30cm"; "a fixed 3-minute training"; "The whole experiment lasted approximately 35 minutes per participant" (p. 4).
- p. 4: "removed outliers where selection time exceeded M ± 3 · SD in each condition (41 trials, ≈1.90%)".
- p. 4, selection time: "Technique (F2,1977 = 70.587, p< .001, η2p= .067) and Density (F1,1977 = 50.104, p< .001, η2p= .025) had a significant main effect"; interaction "(F2,1977 = 8.887, p< .001, η2p= .010)". High density: "HCG (M= 4.18s, SD= 1.51s) was significantly slower than HDC (M= 4.05s, SD= 2.75s) and MFB (M= 3.76s, SD= 1.96s)… (p< .001 for both)". Low density: "HCG (M= 3.80s, SD= 1.44s) was significantly slower than HDC (M= 3.03s, SD= 1.94s; p< .001) and MFB (M= 3.68s, SD= 2.09s; p= .001)"; "HDC was faster than MFB (p< .001)"; density effects "(p< .001 and p= .007, respectively)".
- p. 4, success rate: "Technique (F2,85 = 45.3170, p< .001, η2p= .516) and Density (F1,85 = 21.649, p< .001, η2p= .203)"; interaction "(F2,85 = 5.734, p= .005, η2p= .119)". High density: "HCG (M= 98.83%, SD= 2.96%)… HDC (M= 93.17%, SD= 5.59%; p< .001) and MFB (M= 87.44%, SD= 8.77%; p< .001)". Low density: "HCG (M= 99.72%, SD= 1.18%) and HDC (M= 97.68%, SD= 5.46%)… than MFB (M= 89.33%, SD= 7.75%) (p< .001 for both)"; "the success rate by using HDC decreased 4.51% when the density increased (p= .002)".
- p. 4, UEQ-S: "significant main effect on pragmatic quality (χ2 2 = 8.943, p= .011, W= .248), but not on hedonic quality and overall quality (p=.097 and .662, respectively). Pairwise tests did not yield any significant differences on pragmatic quality (p> .05)".
- pp. 4–5, NASA-TLX: "mental demands (χ2 2 = 13.759, p= .001, W= .382), physical demands (χ2 2 = 11.446, p= .003, W= .318), temporal demands (χ2 2 = 12.043, p= .002, W= .335), effort (χ2 2 = 7.483, p= .024, W= .208), frustration (χ2 2 = 8.400, p= .015, W= .233), and overall workload (χ2 2 = 14.111, p< .001, W= .392)"; "Performance (χ2 2 = 5.719, p= .057, W= .159)"; "MFB (Mdn= 44.44) led to a significantly higher workload than HDC (Mdn= 25.00; p= .021) and HCG (Mdn= 20.83; p= .003)".
- p. 5, Borg CR10: "significant main effect of Technique (χ2 2 = 10.714, p= .005, W= .298)… MFB (Mdn= 2.50) induced more arm fatigue than HDC (Mdn= 1.00; p= .032) and HCG (Mdn= 1.00; p= .026)".
- p. 5, preference: "11 participants (61%) ranked HCG as the most favored technique, and 10 (55.56%) ranked HCG as the second. For the third place… 5 participants (27.28%) chose HDC, 5 chose (27.28%) HCG, and 8 (44.44%) chose MFB".
- p. 5: "More than half (N= 10) of participants ranked HDC in second place"; "four participants complained the moving speed of the depth cursor was too slow"; p. 6: "Seven participants reported unintended activations"; "The four participants who ranked MFB as the most preferred technique"; "six participants mentioned they were not able to flex their middle or ring fingers".
- p. 4: "Shapiro-Wilk tests and Q-Q plots indicated that both performance measures were non-normally distributed. Thus, we pre-processed the data through Aligned Rank Transform [10, 34] before performing two-way repeated-measure (RM-) ANOVA tests. Pairwise comparisons were conducted with Bonferroni corrections"; Friedman tests for subjective measures (p. 4); "Bonferroni-adjusted significance level of .05, .01, and .001" (Figure 2 caption, p. 5).

**How the system decides what to show the user** — Overlay content is determined by hand tracking and the disambiguation mechanism, not by a knowledge source: "HDC uses raycasting as the selection mechanism. The user can point to the target using her dominant hand, and perform selection via a pinch gesture… we also let the objects become transparent if they are behind the cursor" (p. 3); "HCG starts with cone-casting… By spreading or closing her non-dominant hand… the user can adjust the size of the volume… All objects in the defined volume are listed on a grid" (p. 3); "MFB would associate the object candidates with the fingers… we used the index, middle, and ring fingers, and visualized the mapping lines between candidate objects and fingers in red, green, and blue… we adopted the stable mapping strategy; that is, the newly entered object would take the finger assignment and its color indication from the exited object" (p. 3); scene generation: "We used Poisson disk sampling [5] to randomize the positions of spherical objects" (p. 4).

**What the authors say is missing from the field or from practice** — "However, limited research has focused on selection via mid-air barehand, which represents another major input metaphor for immersive environments" (p. 1); "Like SQUAD, most enhancement techniques are designed for and evaluated with handheld controllers… There has been less research that has looked into bare-hand mid-air input for VR" (p. 2); "This leads to a gap in bare-hand pointing selection for dense VR environments" (p. 2); "MultiFingerBubble [9], the only bare-hand pointing technique we found for target selection in dense VR environments" (p. 3); "Seven participants reported unintended activations (due to unintentional finger flexes), which were not reported or described in detail by [9]" (p. 6).

**Authors' own stated limitations and future work** — Section 6.3 (p. 6): "this work has two limitations, which represent possible avenues for future work. First, the MultiFingerBubble [9] was first proposed and implemented using haptic gloves, while we used the headset's built-in cameras to track the hands as a bare-hand approach. The difference in tracking approach may have led to slightly different results. Second, based on the taxonomy [1, 42], we proposed two techniques with defined gestures. In the future, we want to explore other possible gestures for the proposed techniques, enhance the techniques, and compare them with the controller-based techniques… We plan to explore the possibility of combining bare-hand and on-body techniques with suitable disambiguation mechanisms in the future. In addition, we plan to test further optimized techniques in real VR scenarios and with other properties of the objects (e.g., non-regular, arbitrary shapes) and mixed with non-selectable environmental objects as distractors."

**Five verbatim headline claims**
1. "Our results suggest that HandDepthCursor and HandConeGrid led to significantly faster and more accurate selection performance, and lower perceived workload and arm fatigue." (p. 1)
2. "In addition, HandConeGrid showed a distinct advantage in high-density environments." (p. 1)
3. "MultiFingerBubble, though faster than the other two, generated more errors and required more effort." (p. 2)
4. "L1. HandDepthCursor and HandConeGrid are suggested due to their higher accuracy, lower workload, and arm fatigue." (p. 6)
5. "L3. Designers should consider the capabilities of hand-tracking devices. In this study, we found finger actions might be blocked by other parts of the hand, which affected user performance." (p. 6)

**Internal inconsistencies**
- Speed claim vs measured times: "HandDepthCursor and HandConeGrid led to significantly faster and more accurate selection performance" (p. 1) against "HCG (M= 4.18s, SD= 1.51s) was significantly slower than HDC… and MFB… in high density (p< .001 for both)" (p. 4) and "Though it was statistically slower than the other two (see Figure 2a)" (p. 5).
- Preference tallies exceed the sample: "11 participants (61%) ranked HCG as the most favored technique, and 10 (55.56%) ranked HCG as the second" (p. 5), i.e. 21 of 18 participants; the second figure conflicts with "More than half (N= 10) of participants ranked HDC in second place" (p. 5).
- Percentage arithmetic: "5 participants (27.28%)" and "5 chose (27.28%)" (p. 5) where 5/18 = 27.78%.
- Vision reporting: "We recruited 18 participants… Six of them had normal or corrected-to-normal vision" (p. 3), leaving the visual status of the remaining twelve unstated while all completed a visual selection task.
- Degrees of freedom vs trial count: "we collected 2160 trials of data" and "removed outliers… (41 trials, ≈1.90%)" (p. 4), leaving 2119 trials, against the reported "F2,1977" and "F1,1977" (p. 4).

---

## 5. `Augmented reality-enabled knowledge management.pdf`

**Identification** — Title: "Augmented reality-enabled knowledge management in industrial maintenance: the DILEAF framework". Authors: Wanting Mao (corresponding, Dept. of Civil and Environmental Engineering, Imperial College London), Sara Scheffer (TU Wien, Research Unit of Production and Maintenance Management), Arnab Majumdar (Imperial College London). Year: 2025. Venue: *Computers & Industrial Engineering*, volume 208 (2025), article 111363. DOI: `https://doi.org/10.1016/j.cie.2025.111363`. **One sentence:** It proposes the DILEAF (Data, Information, Learning, Engagement, Application, Feedback) knowledge-management framework with AR as digital enabler and validates it through a HoloLens 2 prototype tested in one laboratory and two field iterations on the SNT250 sanitary-system fault at Nederlandse Spoorwegen.

**Citation as printed** — Mao, W., Scheffer, S., Majumdar, A. (2025), "Augmented reality-enabled knowledge management in industrial maintenance: the DILEAF framework", *Computers & Industrial Engineering* 208 (2025) 111363; DOI `10.1016/j.cie.2025.111363`; "Received 18 February 2025; Received in revised form 16 May 2025; Accepted 1 July 2025"; "Available online 2 July 2025"; "0360-8352/© 2025 The Author(s). Published by Elsevier Ltd… open access article under the CC BY-NC-ND license" (p. 1). Issue: not stated; page range: not stated (article number 111363). Peer-reviewed journal article, **not a pre-print**; no arXiv stamp. PDF metadata matches, including the subject field "Computers & Industrial Engineering, 208 (2025) 111363. doi:10.1016/j.cie.2025.111363".

**Type of work, in the paper's own words** — "this study introduces the Data, Information, Learning, Engagement, Application, Feedback (DILEAF) framework" (p. 1); "This study utilises the Design Science Research Methodology (DSRM)" (p. 4); "The artefact designed in this study is the DILEAF framework" (p. 4); "This study provides both a theoretical and empirical contribution" (p. 2).

**What was studied** — Task: "the AR-based SNT250 calibration experiments" (p. 10) — temperature-sensor calibration on the sanitary system/bioreactor of VIRM double-decker intercity trains; 22 steps listed in Table 5 (pp. 14–15). System: "The AR system is deployed on the Microsoft HoloLens 2… The AR interface was built using Unity 3D, with Microsoft's Mixed Reality Toolkit (MRTK)… The backend knowledge engine is hosted on Microsoft Azure… Real-time communication between the frontend and backend is achieved via RESTful APIs" (p. 7). Participants: "Semi-structured interviews were conducted with four experienced technicians, each with over a decade of experience… at an NS maintenance facility" (p. 9); "eleven technicians (N = 11) from the Onnen and Maastricht maintenance facilities participated… i) eleven were required to be specialised in troubleshooting system failures of the sanitary system, and ii) none were to have any prior experience with AR technology" (p. 10). Allocation: "The initial laboratory-based experiment involved five technicians… In the first field iteration, three participants worked with real-time data… The second field iteration introduced three technicians" (p. 10). Conditions compared: three sequential iterations (lab with "Real-time data from Arduino", field iteration 1, field iteration 2), Table 3 (p. 12); there is no control group and no AR-versus-paper comparison.

**Every quantitative result**
- p. 1: "The Maintenance, Repair, and Operations (MRO) market was valued at USD 430.98 billion in 2024 and is expected to grow at a compound annual growth rate (CAGR) of 2.28 %, reaching USD 482.40 billion by 2029"; "91 % of participants in the case study expressed satisfaction with the clarity and usefulness of the information presented via AR".
- p. 2: maintenance costs "often representing 50 % to 70 % of production expenditures".
- p. 3: "Palmarini et al. (2023) developed the Fast AR Authoring (FARA) method, which reduced task completion time by an average of 34.7 % and errors by 68.6 % compared to traditional manuals" (result of a cited study, not of this study).
- p. 9: "the SNT250 as one of the top twenty most frequently failing systems in trains"; "Each interview was approximately one hour in duration"; "On average, a basic analysis of failures typically takes one to one and a half hours, while preparation, information review, and preliminary tasks generally add an extra half hour… can extend to more than five hours".
- p. 10: N = 11; 5 + 3 + 3 participants across three rounds.
- p. 13: "All participants, regardless of prior experience with AR, successfully completed the assigned maintenance procedures on the SNT250 system"; "Error rates remained low, and 45 % of the participants needed information on related subcomponents for task completion"; "In total, 91 % of participants were satisfied with the information presented in AR".
- pp. 14–15, Table 5: 22 numbered calibration steps; "Ensure the baud rate is set to 115200.n.8.1"; "Enter code '2633'"; "Click 'OK' four times"; "initial temperature values Tw and Tr are set to 0".
No statistical tests are reported; the paper states "the current evaluation primarily emphasises thematic and perception-based feedback" (p. 13).

**How the system decides what to show the user** — "Real-time data was captured directly from IoT-enabled sensors, such as temperature fluctuations and vibration anomalies… Historical maintenance records were manually uploaded into the backend knowledge base in structured JSON format, including component IDs, timestamps, and fault descriptions. These datasets were stored in a cloud database (Azure Cosmos DB), where they could be retrieved by the AR system via API calls" (p. 7). "For each fault trigger or component reference, the system called a corresponding maintenance procedure via API requests to the backend knowledge base. The procedures were stored in structured JSON format and included stepwise instructions, visual highlights, and simple schematic diagrams. The AR interface parsed this information and displayed relevant instructions using Unity and MRTK, with holographic overlays anchored to real equipment through spatial mapping" (p. 8). "Each instruction remained active until the technician confirmed completion using gesture-based input, after which the system advanced to the next step" (p. 8). "a cloud-based repository stores structured knowledge objects defined by a domain ontology" (p. 7). "Items such as procedures, diagnostics, and sensor data are categorised by equipment type, failure mode, and task phase" (p. 6). "This is supported by context-aware delivery mechanisms, such as task-phase detection and AI-driven scene recognition, which enable content to adapt dynamically to the user's working conditions" (p. 6). "Frequently flagged steps are marked for review using a simple classification scheme (e.g., 'unclear' or 'outdated'), triggering content updates by designated maintainers" (p. 8).

**What the authors say is missing from the field or from practice** — "most current KM systems fail to offer real-time contextual support or align with the dynamic, task-specific demands of field technicians. Knowledge is often captured and stored post-task, leading to significant time lags" (p. 1); "these studies focus primarily on AR as an auxiliary tool rather than as a core mechanism for knowledge creation, contextual learning, and iterative refinement… it has not yet been embedded into end-to-end knowledge cycles" (p. 2); the three gaps (pp. 3–4): "i. Absence of a holistic KM framework"; "ii. Limited empirical validation in field environments — A significant portion of existing research on AR for KM in maintenance is based on controlled laboratory settings or conceptual models"; "iii. Insufficient evaluation of AR's functional impact on KM… Existing studies often treat AR as a generalised technological enhancement". On practice: "each maintenance facility manages SharePoint independently, leading to inconsistencies… This lack of a unified system restricts knowledge sharing across facilities" (p. 9); "'…half the time, the reports do not match reality.'" (p. 9); "The findings from the railway industry revealed a lack of consistent feedback mechanisms in existing systems" (p. 13).

**Authors' own stated limitations and future work** — "This study has several limitations… The sample size was relatively small, and the experiments were conducted within a specific industrial context, which may limit the generic nature of the findings. Future research should explore the scalability of the DILEAF framework across different industries and maintenance environments, including its adaptation to larger teams and enterprise-level AR infrastructures. Also, further investigation into the long-term impact of AR on KM, KT and organisational learning is warranted. Expanding the scope of feedback integration to include AI-driven analytics, predictive modelling, and interoperability with digital twins… Future research may explore adaptive AR interfaces that respond to user experience levels and task complexity, alongside the application of cognitive design principles… the implementation of appropriate cybersecurity and data protection mechanisms will be essential" (pp. 13–14). Also: "the current evaluation primarily emphasises thematic and perception-based feedback" (p. 13); "field experiments revealed that despite improvements in data accessibility, inconsistencies remain in real-world contexts, calling for enhanced interoperability between AR systems and enterprise databases" (p. 13); "Key challenges included visual misalignment due to lighting variation, anchor drift in complex environments, and inconsistent interaction patterns" (p. 13); "improvements are needed to maintain data completeness and accessibility in more dynamic field contexts" (p. 11). "The data that has been used is confidential" (p. 15).

**Five verbatim headline claims**
1. "It was shown that AR overlays play a crucial role in enabling early error detection and correction, directly supporting the overall task success rate." (p. 1)
2. "Furthermore, 91 % of participants in the case study expressed satisfaction with the clarity and usefulness of the information presented via AR, underscoring the framework's effectiveness in delivering task-relevant knowledge and supporting robust performance in maintenance scenarios." (p. 1)
3. "This study's novelty lies in systematically embedding AR within a holistic KM framework that spans the entire knowledge lifecycle, from data capture to feedback, thus enabling intelligent support systems that evolve with user interaction and task complexity." (p. 2)
4. "By comparison, this study offers one of the first validated and user-centred KM models developed for dynamic maintenance environments." (p. 3)
5. "The findings demonstrate that AR-enabled KM significantly enhances procedural accuracy, decision-making efficiency, and user engagement, addressing practical challenges such as fragmented information access and inconsistent procedural adherence." (p. 14)

**Internal inconsistencies**
- Prior AR experience: selection criterion "ii) none were to have any prior experience with AR technology" (p. 10) against "All participants, regardless of prior experience with AR, successfully completed the assigned maintenance procedures" (p. 13) and against survey item "SQ5 Do you have experience or knowledge of using AR?" (p. 5).
- Percentage against sample size: "45 % of the participants needed information on related subcomponents" and "91 % of participants were satisfied" (p. 13) with N = 11 (p. 10), where 45 % of 11 = 4.95 participants.
- Count of prioritised functions: "Table 2 summarises the outcomes of this prioritisation, highlighting four key functions deemed vital for the framework's design" (p. 7) against Table 2, which lists five requirements — Structured Knowledge Categorisation (Medium), Centralised Data Integration (High), Interactive Procedural Guidance (High), Contextual Knowledge Adaptability (Medium), Iterative Feedback Mechanisms (High) (p. 7) — and against Section 4.2 Step 1, which lists five prioritised functions (p. 6).
- Strength of evidence: "The findings demonstrate that AR-enabled KM significantly enhances procedural accuracy, decision-making efficiency, and user engagement" (p. 14) and "improvements in task accuracy" (p. 1) against "the current evaluation primarily emphasises thematic and perception-based feedback" (p. 13); no statistical test is reported anywhere.
- Duplicated clause: "current systems are often poorly integrated at the operational level, leading to fragmented KT (Keiser et al., 2023), leading to fragmented KT" (p. 2).
- Garbled gap statement: "(3) insufficient knowledge between knowledge structuring and interactive guidance" (p. 2).
- Section heading "5.2.3. Troubleshooting produce" (p. 10) against its content, which describes the troubleshooting procedure.agentId: ac08f6f9bbb21b967 (use SendMessage with to: 'ac08f6f9bbb21b967', summary: '<5-10 word recap>' to continue this agent)
<usage>subagent_tokens: 203179
tool_uses: 16
duration_ms: 334916</usage>