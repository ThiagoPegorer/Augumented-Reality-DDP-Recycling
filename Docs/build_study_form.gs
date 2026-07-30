/**
 * AR-DPP User Study — Google Forms builder (RBv1.0, questionnaire v1.1)
 *
 * HOW TO RUN (once):
 *   1. Go to https://script.google.com → New project.
 *   2. Delete the default code, paste this whole file, save.
 *   3. Run ▶ buildForm (authorize with your Google account when asked).
 *   4. Open View → Logs (Executions) — it prints the EDIT and RESPOND links.
 *   5. MANUAL FINISHING STEP (API can't do it): open the form editor, go to the
 *      LAST section ("Experimenter — dismantling report"), add a question →
 *      type "File upload" → allow 1 file, 10 MB. Done.
 *
 * Note: adding the file-upload question makes Google require respondent
 * sign-in for the whole form (files go to your Drive). Fine on the lab
 * laptop with your account signed in.
 */

function buildForm() {
  var form = FormApp.create('AR-DPP User Study — Guided Disassembly (RBv1.0)');
  form.setDescription(
    'Comparison study: conventional 2D manual vs AR Digital Product Passport.\n' +
    'You will use BOTH versions. Your experimenter will tell you when to fill each page.\n' +
    'No personal data is collected — only your participant ID.');
  form.setProgressBar(true);
  form.setShowLinkToRespondAgain(false);

  var SCALE_DISAGREE = ['Strongly disagree', 'Strongly agree'];

  // ---------- Section 1 — Participant (first page, no break needed) ----------
  form.addSectionHeaderItem().setTitle('Participant')
      .setHelpText('Fill this BEFORE starting the tasks.');

  form.addTextItem().setTitle('Participant ID (given by the experimenter)').setRequired(true);

  form.addMultipleChoiceItem().setTitle('Age group')
      .setChoiceValues(['18–24', '25–34', '35–44', '45+']).setRequired(true);

  form.addTextItem().setTitle('Field of study / profession').setRequired(true);

  scale(form, 'Prior experience with VR/AR headsets', 'None', 'Very experienced');
  scale(form, 'Prior experience disassembling electronic devices', 'None', 'Very experienced');

  // ---------- Section 2 — SUS: 2D manual ----------
  form.addPageBreakItem().setTitle('Your experience with the CONVENTIONAL 2D MANUAL')
      .setHelpText('Fill this page immediately AFTER completing the task with the 2D manual. ' +
                   '1 = Strongly disagree · 5 = Strongly agree.');
  susBlock(form, [
    'I think that I would like to use this manual frequently for tasks like this.',
    'I found the manual unnecessarily complex.',
    'I thought the manual was easy to use.',
    'I think that I would need the support of a technical person to be able to use this manual.',
    'I found the various parts of the manual well integrated.',
    'I thought there was too much inconsistency in this manual.',
    'I would imagine that most people would learn to use this manual very quickly.',
    'I found the manual very cumbersome to use.',
    'I felt very confident using the manual.',
    'I needed to learn a lot of things before I could get going with this manual.'
  ], SCALE_DISAGREE);

  // ---------- Section 3 — SUS: AR application ----------
  form.addPageBreakItem().setTitle('Your experience with the AR APPLICATION')
      .setHelpText('Fill this page immediately AFTER completing the task with the AR application. ' +
                   '1 = Strongly disagree · 5 = Strongly agree.');
  susBlock(form, [
    'I think that I would like to use this AR application frequently for tasks like this.',
    'I found the AR application unnecessarily complex.',
    'I thought the AR application was easy to use.',
    'I think that I would need the support of a technical person to be able to use this AR application.',
    'I found the various functions of the AR application well integrated.',
    'I thought there was too much inconsistency in this AR application.',
    'I would imagine that most people would learn to use this AR application very quickly.',
    'I found the AR application very cumbersome to use.',
    'I felt very confident using the AR application.',
    'I needed to learn a lot of things before I could get going with this AR application.'
  ], SCALE_DISAGREE);

  // ---------- Section 4 — Direct comparison ----------
  form.addPageBreakItem().setTitle('Comparing the two versions')
      .setHelpText('For each aspect, choose which version worked better for you. ' +
                   '1 = definitely the CONVENTIONAL 2D MANUAL · 3 = no difference · ' +
                   '5 = definitely the AR 3D MODEL. A 2 or 4 means a slight preference.');
  var CMP = ['Conventional 2D manual', 'AR 3D model'];
  [
    'Which version let you work with more agility (faster, smoother progress)?',
    'Which version made the instructions easier to understand?',
    'Which version made it easier to identify the correct component at each step?',
    'Which version gave you more confidence that you were doing the step correctly?',
    'Which version helped you better avoid mistakes (wrong part, wrong order)?',
    'Which version required less mental effort to follow?',
    'Which version taught you more about what the components are made of and their value (materials, recovery)?',
    'With which version would you feel more prepared to disassemble a similar device again without help?',
    'Which version was more engaging to use?',
    'Overall, which version would you choose for this kind of task?'
  ].forEach(function (q) { scale(form, q, CMP[0], CMP[1]); });

  // ---------- Section 5 — Open questions ----------
  form.addPageBreakItem().setTitle('In your own words')
      .setHelpText('Short answers are fine — honest ones are better.');
  para(form, 'What did you find most helpful in the AR application?', true);
  para(form, 'What was most difficult or frustrating in the AR application?', true);
  para(form, 'Was there a moment where the 2D manual worked better for you than the AR version? Describe it.', false);
  para(form, 'How intuitive were the hand gestures (rotate, zoom, moving parts)? What would you change?', false);
  para(form, 'Did the material/recovery information (what parts are worth recovering, and why) influence how you did the task? How?', false);
  para(form, 'If this tool existed at a real recycling workstation, would you want to use it? Why / why not?', false);
  para(form, 'Anything else you want to tell us?', false);

  // ---------- Section 6 — Experimenter: report upload (placeholder) ----------
  form.addPageBreakItem().setTitle('Experimenter — dismantling report')
      .setHelpText('EXPERIMENTER ONLY: attach this participant’s dismantling report (.json) ' +
                   'exported from the backend.');
  form.addSectionHeaderItem().setTitle('⚠ ADD THE FILE-UPLOAD QUESTION HERE MANUALLY')
      .setHelpText('Google’s API cannot create upload questions. In the form editor: ' +
                   '+ Add question → File upload → 1 file, 10 MB. Then DELETE this placeholder.');

  Logger.log('EDIT the form:    ' + form.getEditUrl());
  Logger.log('RESPOND (share):  ' + form.getPublishedUrl());
}

// ---- helpers ----
function scale(form, title, left, right) {
  form.addScaleItem().setTitle(title).setBounds(1, 5)
      .setLabels(left, right).setRequired(true);
}
function susBlock(form, questions, labels) {
  questions.forEach(function (q) { scale(form, q, labels[0], labels[1]); });
}
function para(form, title, required) {
  form.addParagraphTextItem().setTitle(title).setRequired(required);
}
