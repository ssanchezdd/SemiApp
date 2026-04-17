using SemiologyAtlas.Models;

namespace SemiologyAtlas.Services;

public static class CurriculumRepository
{
    public static IReadOnlyList<LearningPath> GetLearningPaths()
    {
        return
        [
            new LearningPath(
                T("Encuesta general", "General survey"),
                T(
                    "Aprende a describir el aspecto general en 30 segundos: estado de alerta, postura, coloracion, hidratacion, movilidad y vestido.",
                    "Learn to describe general appearance in 30 seconds: alertness, posture, color, hydration, mobility, and dress."),
                T("5 modulos", "5 modules"),
                "//library"),
            new LearningPath(
                T("Laboratorio de movimiento", "Movement lab"),
                T(
                    "Diferencia sindromes hipocineticos e hipercineticos con lenguaje de cabecera y ejemplos semiologicos clasicos.",
                    "Differentiate hypokinetic and hyperkinetic syndromes with bedside language and classic semiologic examples."),
                T("9 patrones", "9 patterns"),
                "//movement"),
            new LearningPath(
                T("Repaso para quiz", "Quiz review"),
                T(
                    "Revisa los conceptos de alto rendimiento y luego confirma tu reconocimiento de patrones con preguntas clinicas.",
                    "Review high-yield concepts and then confirm your pattern recognition with clinical questions."),
                T("8 preguntas", "8 questions"),
                "//quiz")
        ];
    }

    public static IReadOnlyList<string> GetTeachingRules()
    {
        return Lines(
            ("Describe primero el signo visible y solo despues nombra el sindrome.", "Describe the visible sign first and only then name the syndrome."),
            ("En movimiento anormal, decide antes si hay poco movimiento, demasiado movimiento o un problema de la marcha.", "In abnormal movement, decide first whether there is too little movement, too much movement, or a gait problem."),
            ("Cada diagnostico debe quedar anclado a una maniobra reproducible de exploracion fisica.", "Every diagnosis should be anchored to a reproducible bedside maneuver."),
            ("Separa aspecto general, hallazgos clave, banderas rojas y errores frecuentes en tu presentacion clinica.", "Separate general appearance, key findings, red flags, and common mistakes in your clinical presentation."));
    }

    public static IReadOnlyList<StudyGuideCard> GetStudyGuideCards()
    {
        return
        [
            new StudyGuideCard(
                T("Puerta antes de tocar", "Doorway before touch"),
                T("Antes del examen focal, practica describir alerta, distres, coloracion, hidratacion y vestido.", "Before the focused exam, practice describing alertness, distress, color, hydration, and dress."),
                "//library",
                "general_survey"),
            new StudyGuideCard(
                T("Bradicinesia y rigidez", "Bradykinesia and rigidity"),
                T("Repasa decremento secuencial y resistencia uniforme no dependiente de velocidad.", "Review sequential decrement and uniform resistance that is not velocity-dependent."),
                "//movement",
                "bradykinesia"),
            new StudyGuideCard(
                T("Reposo vs accion", "Rest vs action"),
                T("Para diferenciar temblor parkinsoniano y temblor esencial, primero define si aparece en reposo o con postura/accion.", "To separate parkinsonian tremor from essential tremor, first define whether it appears at rest or with posture/action."),
                "//movement",
                "parkinson_tremor"),
            new StudyGuideCard(
                T("Marcha parkinsoniana", "Parkinsonian gait"),
                T("Describe base, longitud del paso, braceo, giros y congelamiento.", "Describe base, stride length, arm swing, turns, and freezing."),
                "//movement",
                "parkinson_gait"),
            new StudyGuideCard(
                T("Bandera roja respiratoria", "Respiratory red flag"),
                T("Torax silencioso mas uso de musculos accesorios e incapacidad para hablar es alto riesgo.", "Silent chest plus accessory muscles and inability to speak is high risk."),
                "//library",
                "resp_pattern"),
            new StudyGuideCard(
                T("Triada de congestion", "Congestion triad"),
                T("Yugular elevada, crepitos bibasales y edema periferico favorecen insuficiencia cardiaca congestiva.", "Raised JVP, bibasal crackles, and peripheral edema favor congestive heart failure."),
                "//library",
                "cardio_clues")
        ];
    }

    public static IReadOnlyList<SystemModule> GetSystemModules()
    {
        return
        [
            new SystemModule(
                "general_survey",
                T("General", "General"),
                T("Aspecto general y primera impresion", "General appearance and first impression"),
                T(
                    "Observa alerta, distres, postura, perfusion y vestido antes del examen focal.",
                    "Observe alertness, distress, posture, perfusion, and dress before the focused exam."),
                T(
                    "La semiologia general empieza antes del contacto. Observa habito corporal, nivel de conciencia, trabajo respiratorio, hidratacion, higiene, postura, marcha de ingreso y adecuacion del vestido al contexto.",
                    "General semiology starts before contact. Observe habitus, level of consciousness, work of breathing, hydration, hygiene, posture, gait on entry, and whether clothing fits the clinical context."),
                T(
                    "Que te dice la primera impresion sobre estabilidad, carga de enfermedad y vulnerabilidad social?",
                    "What does the first impression tell you about stability, disease burden, and social vulnerability?"),
                "general_survey",
                Lines(
                    ("Detente en la puerta y describe edad aparente, estado nutricional y nivel de alerta.", "Pause at the doorway and describe apparent age, nutritional status, and alertness."),
                    ("Observa postura antalgica, disnea, uso de musculos accesorios, sudoracion o agitacion.", "Look for antalgic posture, dyspnea, accessory muscle use, diaphoresis, or agitation."),
                    ("Valora coloracion, perfusion periferica, hidratacion de mucosas y olor corporal.", "Assess color, peripheral perfusion, mucosal hydration, and body odor."),
                    ("Incluye vestido, aseo, cooperacion y forma de movilizarse en la habitacion.", "Include dress, grooming, cooperation, and how the patient moves around the room.")),
                Points(
                    ("Habito corporal", "Habitus", "Caquexia, obesidad, edema o fragilidad ya orientan la carga de enfermedad cronica.", "Cachexia, obesity, edema, or frailty already orient you toward chronic disease burden."),
                    ("Distres", "Distress", "Agitacion, diaforesis, cara toxica o agotamiento respiratorio son reconocedores rapidos de gravedad.", "Agitation, diaphoresis, toxic appearance, or respiratory fatigue are rapid recognizers of severity."),
                    ("Perfusion", "Perfusion", "Palidez, cianosis, ictericia, extremidades frias o mucosas secas cambian la prioridad diagnostica.", "Pallor, cyanosis, jaundice, cool extremities, or dry mucosa immediately change diagnostic priority."),
                    ("Vestido e higiene", "Dress and hygiene", "Ropa inadecuada, mal aseo o incontinencia pueden sugerir deterioro cognitivo, enfermedad psiquiatrica o abandono.", "Inappropriate clothing, poor grooming, or incontinence may suggest cognitive decline, psychiatric disease, or neglect.")),
                Points(
                    ("Perla clave", "High-yield pearl", "Presenta siempre un resumen de una linea: paciente consciente, adelgazado, con disnea leve y mala higiene.", "Always present a one-line summary: alert patient, thin, mildly dyspneic, with poor hygiene."),
                    ("Que aprender", "What to learn", "El aspecto general responde preguntas de estabilidad antes de que el examen por sistemas empiece.", "General appearance answers stability questions before the system-based exam begins."),
                    ("Como memorizarlo", "Memory anchor", "Puerta, postura, perfusion y presentacion: cuatro P para no olvidar la encuesta general.", "Doorway, posture, perfusion, and presentation: four anchors for the general survey."),
                    ("Relacion con el quiz", "Quiz link", "El quiz pregunta por lo que debe observarse antes de tocar al paciente.", "The quiz asks what belongs to the survey before touching the patient.")),
                Lines(
                    ("Compromiso del estado de conciencia o incapacidad para mantener atencion.", "Reduced level of consciousness or inability to sustain attention."),
                    ("Cianosis, disnea severa o imposibilidad para hablar frases completas.", "Cyanosis, severe dyspnea, or inability to speak in full sentences."),
                    ("Apariencia de shock con moteado, sudor frio y mala perfusion distal.", "Shock appearance with mottling, cold sweat, and poor distal perfusion.")),
                Lines(
                    ("No reduzcas el aspecto general a paciente en regular estado general; describe signos visibles concretos.", "Do not reduce the general survey to fair general condition; describe concrete visible signs."),
                    ("No olvides la movilidad y la marcha espontanea al entrar a la consulta.", "Do not forget spontaneous mobility and gait as the patient enters."),
                    ("Evita mezclar hallazgos del examen focal con la encuesta general.", "Avoid mixing focused exam findings into the general survey."))),
            new SystemModule(
                "neurologic_screen",
                T("Neurologico", "Neurological"),
                T("Tamizaje neurologico y fenomenologia del movimiento", "Neurological screening and movement phenomenology"),
                T(
                    "Tamizaje rapido de cara, manos, tono, temblor y marcha para reconocer patrones neurologicos.",
                    "Rapid screening of face, hands, tone, tremor, and gait to recognize neurologic patterns."),
                T(
                    "Organiza el examen en estado mental, pares craneales, sistema motor, coordinacion, tono, movimientos involuntarios y marcha. Despues decide si el problema dominante es hipocinesia, hipercinesia o un deficit focal.",
                    "Organize the exam into mental status, cranial nerves, motor system, coordination, tone, involuntary movements, and gait. Then decide whether the dominant problem is hypokinesia, hyperkinesia, or a focal deficit."),
                T(
                    "Predomina la lentitud con pobreza de movimiento, el exceso de movimiento involuntario o un sindrome focal agudo.",
                    "Does slowness with paucity of movement, excess involuntary movement, or an acute focal syndrome dominate."),
                "neurologic_screen",
                Lines(
                    ("Observa mimica facial, parpadeo, volumen de voz y gesticulacion espontanea.", "Observe facial expression, blink rate, voice volume, and spontaneous gesturing."),
                    ("Explora finger tapping, abrir-cerrar manos y prono-supinacion rapida buscando decremento.", "Examine finger tapping, hand opening-closing, and rapid pronation-supination looking for decrement."),
                    ("Palpa tono pasivo en muneca, codo y cuello; diferencia rigidez de espasticidad.", "Assess passive tone at wrist, elbow, and neck; distinguish rigidity from spasticity."),
                    ("Describe marcha por base, longitud del paso, braceo, giros y congelamiento.", "Describe gait by base, stride length, arm swing, turning, and freezing.")),
                Points(
                    ("Bradicinesia", "Bradykinesia", "No es solo lentitud: la amplitud y velocidad se degradan a lo largo de la secuencia.", "It is not just slowness: amplitude and speed degrade over the sequence."),
                    ("Rigidez", "Rigidity", "La resistencia es uniforme y no depende de la velocidad del movimiento pasivo.", "Resistance is uniform and does not depend on the speed of passive movement."),
                    ("Temblor por estado", "Tremor by state", "Clasifica si es de reposo, postural, cinetico, de intencion o especifico de tarea.", "Classify tremor as rest, postural, kinetic, intention, or task-specific."),
                    ("Marcha", "Gait", "Un giro en bloque, paso corto y menor braceo orientan a marcha parkinsoniana.", "En bloc turning, short steps, and reduced arm swing point toward parkinsonian gait.")),
                Points(
                    ("Perla clave", "High-yield pearl", "En trastornos del movimiento, el nombre del fenomeno sale despues de describir velocidad, ritmo, amplitud y patron.", "In movement disorders, the phenomenon's name comes after describing speed, rhythm, amplitude, and pattern."),
                    ("Que aprender", "What to learn", "La division hipocinetico vs hipercinetico organiza casi todo el laboratorio de movimiento.", "The hypokinetic vs hyperkinetic split organizes most of the movement lab."),
                    ("Como memorizarlo", "Memory anchor", "Cara, manos, tono, temblor y marcha: cinco estaciones del tamizaje neurologico rapido.", "Face, hands, tone, tremor, and gait: five stations of the rapid neurological screen."),
                    ("Relacion con el quiz", "Quiz link", "Las preguntas sobre bradicinesia, rigidez, temblor parkinsoniano y marcha salen de este modulo.", "Questions about bradykinesia, rigidity, parkinsonian tremor, and gait come from this module.")),
                Lines(
                    ("Deficit focal de inicio agudo, afasia, desviacion facial o nueva hemiparesia.", "Acute focal deficit, aphasia, facial droop, or new hemiparesis."),
                    ("Primera convulsion, rigidez de nuca o deterioro rapido del estado mental.", "First seizure, neck stiffness, or rapidly worsening mental status."),
                    ("Caidas repetidas, perdida brusca de la marcha o inestabilidad autonomica marcada.", "Repeated falls, sudden loss of gait, or marked autonomic instability.")),
                Lines(
                    ("No llames temblor esencial a un temblor sin definir antes si aparece en reposo o en accion.", "Do not call tremor essential before defining whether it appears at rest or with action."),
                    ("No confundas espasticidad piramidal con rigidez extrapiramidal.", "Do not confuse pyramidal spasticity with extrapyramidal rigidity."),
                    ("No informes la marcha como normal sin describir giros y balance postural.", "Do not report gait as normal without describing turns and postural balance."))),
            new SystemModule(
                "cardio_clues",
                T("Cardiovascular", "Cardiovascular"),
                T("Claves cardiovasculares al lado de la cama", "Cardiovascular bedside clues"),
                T(
                    "Usa yugulares, pulso, auscultacion y edema para reconocer congestion y bajo gasto.",
                    "Use JVP, pulse, auscultation, and edema to recognize congestion and low output."),
                T(
                    "La semiologia cardiovascular se sostiene en pulso, presion venosa yugular, auscultacion, perfusion periferica y edema. La meta es definir un sindrome: congestion, bajo gasto, lesion valvular o choque.",
                    "Cardiovascular semiology relies on pulse, jugular venous pressure, auscultation, peripheral perfusion, and edema. The goal is to define a syndrome: congestion, low output, valvular disease, or shock."),
                T(
                    "El paciente esta congestionado, tiene bajo gasto, una lesion valvular dominante o una combinacion de estas.",
                    "Is the patient congested, low output, dominated by a valvular lesion, or a combination of these."),
                "cardio_clues",
                Lines(
                    ("Mira yugulares a 45 grados antes de auscultar el corazon.", "Inspect neck veins at 45 degrees before auscultation."),
                    ("Compara pulso central y distal, temperatura de piel y tiempo de relleno capilar.", "Compare central and distal pulses, skin temperature, and capillary refill."),
                    ("Ausculta focos valvulares y agrega irradiacion, intensidad y calidad del soplo.", "Auscultate valve areas and add radiation, intensity, and quality of the murmur."),
                    ("Busca edema, hepatomegalia y crepitos si sospechas falla cardiaca.", "Look for edema, hepatomegaly, and crackles if you suspect heart failure.")),
                Points(
                    ("Congestion", "Congestion", "Yugulares elevadas, crepitos bibasales y edema en miembros inferiores forman un patron clasico de insuficiencia cardiaca.", "Raised JVP, bibasal crackles, and leg edema form a classic heart failure pattern."),
                    ("Bajo gasto", "Low output", "Pulso debil, piel fria y presion de pulso estrecha sugieren hipoperfusion.", "Weak pulse, cool skin, and narrow pulse pressure suggest hypoperfusion."),
                    ("Lesion valvular", "Valvular lesion", "La irradiacion del soplo y el contorno del pulso ayudan mas que solo la intensidad.", "Murmur radiation and pulse contour help more than intensity alone."),
                    ("Ritmo", "Rhythm", "Un pulso irregularmente irregular obliga a pensar en fibrilacion auricular.", "An irregularly irregular pulse should prompt consideration of atrial fibrillation.")),
                Points(
                    ("Perla clave", "High-yield pearl", "Piensa en sindrome antes que en diagnostico ecocardiografico.", "Think syndrome before echocardiographic diagnosis."),
                    ("Que aprender", "What to learn", "Las triadas semiologicas ahorran tiempo: yugular alta, edema y crepitos es congestion hasta demostrar lo contrario.", "Semiologic triads save time: high JVP, edema, and crackles means congestion until proven otherwise."),
                    ("Como memorizarlo", "Memory anchor", "Pulso, cuello, corazon y piernas: cuatro estaciones del examen cardiovascular rapido.", "Pulse, neck, heart, and legs: four stations of the quick cardiovascular exam."),
                    ("Relacion con el quiz", "Quiz link", "El quiz pregunta por la combinacion yugulares altas, crepitos y edema.", "The quiz asks about high JVP, crackles, and edema.")),
                Lines(
                    ("Hipotension con dolor toracico, sincope o edema pulmonar agudo.", "Hypotension with chest pain, syncope, or acute pulmonary edema."),
                    ("Nuevo soplo con sepsis, fiebre o insuficiencia cardiaca aguda.", "A new murmur with sepsis, fever, or acute heart failure."),
                    ("Signos de choque con extremidades frias y alteracion del sensorio.", "Shock signs with cool extremities and altered mental status.")),
                Lines(
                    ("No describas falla cardiaca sin documentar datos de congestion o bajo gasto.", "Do not label heart failure without documenting congestion or low-output findings."),
                    ("No ignores el pulso por concentrarte solo en la auscultacion.", "Do not ignore the pulse while focusing only on auscultation."),
                    ("No confundas edema cronico aislado con congestion hemodinamica activa.", "Do not confuse isolated chronic edema with active hemodynamic congestion."))),
            new SystemModule(
                "resp_pattern",
                T("Respiratorio", "Respiratory"),
                T("Reconocimiento del patron respiratorio", "Respiratory pattern recognition"),
                T(
                    "Cuenta frecuencia, trabajo ventilatorio y hallazgos auscultatorios para definir gravedad.",
                    "Count respiratory rate, work of breathing, and auscultatory findings to define severity."),
                T(
                    "Cuenta frecuencia respiratoria, mira trabajo ventilatorio y luego usa percusion y auscultacion para decidir si el patron es obstructivo, alveolar, pleural o de agotamiento inminente.",
                    "Count respiratory rate, watch ventilatory work, and then use percussion and auscultation to decide whether the pattern is obstructive, alveolar, pleural, or impending fatigue."),
                T(
                    "Predomina obstruccion, consolidacion, compromiso pleural o fracaso ventilatorio inminente.",
                    "Does obstruction, consolidation, pleural disease, or impending ventilatory failure dominate."),
                "resp_pattern",
                Lines(
                    ("Cuenta respiraciones reales por minuto y no solo una impresion visual rapida.", "Count true respirations per minute and not just a quick visual impression."),
                    ("Evalua si puede hablar frases completas y si usa musculos accesorios.", "Assess whether the patient can speak full sentences and whether accessory muscles are used."),
                    ("Percute ambos hemitorax antes de concluir si hay liquido o consolidacion.", "Percuss both hemithoraces before deciding on fluid or consolidation."),
                    ("Ausculta buscando sibilancias difusas, crepitos focales, silencio auscultatorio o respiracion bronquial.", "Auscultate for diffuse wheeze, focal crackles, silent chest, or bronchial breath sounds.")),
                Points(
                    ("Trabajo respiratorio", "Work of breathing", "Tirajes, aleteo nasal y postura en tripode indican una carga ventilatoria que puede no sostenerse.", "Retractions, nasal flaring, and tripod posture indicate a ventilatory load that may not be sustainable."),
                    ("Patron obstructivo", "Obstructive pattern", "Sibilancias difusas y espiracion prolongada orientan a asma o EPOC.", "Diffuse wheeze and prolonged expiration point to asthma or COPD."),
                    ("Patron alveolar", "Alveolar pattern", "Matidez focal, crepitos y soplo tubario sugieren consolidacion.", "Focal dullness, crackles, and bronchial breathing suggest consolidation."),
                    ("Patron pleural", "Pleural pattern", "Murmullo abolido con matidez intensa y menor expansion sugiere derrame.", "Absent breath sounds with marked dullness and reduced expansion suggest effusion.")),
                Points(
                    ("Perla clave", "High-yield pearl", "Un torax silencioso en un paciente agotado puede ser peor que muchas sibilancias.", "A silent chest in a tiring patient can be worse than lots of wheeze."),
                    ("Que aprender", "What to learn", "Primero decide estabilidad ventilatoria y luego define anatomia.", "First decide ventilatory stability and then define the anatomy."),
                    ("Como memorizarlo", "Memory anchor", "Frecuencia, frase, tiraje y fonendoscopio: cuatro pasos para no perder gravedad.", "Rate, sentence, retractions, and stethoscope: four steps to avoid missing severity."),
                    ("Relacion con el quiz", "Quiz link", "El quiz pregunta por el paciente con musculos accesorios y torax casi silencioso.", "The quiz asks about the patient with accessory muscles and an almost silent chest.")),
                Lines(
                    ("Torax silencioso, somnolencia o incapacidad para completar frases.", "Silent chest, drowsiness, or inability to complete sentences."),
                    ("Hipoxemia evidente, cianosis o agotamiento respiratorio.", "Obvious hypoxemia, cyanosis, or respiratory fatigue."),
                    ("Desviacion traqueal o abolicion unilateral subita del murmullo.", "Tracheal deviation or sudden unilateral absence of breath sounds.")),
                Lines(
                    ("No llames asma leve a un paciente que ya no puede hablar con normalidad.", "Do not call it mild asthma when the patient can no longer speak normally."),
                    ("No ignores la percusion cuando sospechas derrame o consolidacion.", "Do not ignore percussion when you suspect effusion or consolidation."),
                    ("No subestimes la frecuencia respiratoria: suele ser el signo vital mas olvidado.", "Do not underestimate respiratory rate: it is often the most neglected vital sign."))),
            new SystemModule(
                "gi_survey",
                T("Gastrointestinal", "Gastrointestinal"),
                T("Abdomen, hepatologia y pistas sistemicas", "Abdomen, hepatology, and systemic clues"),
                T(
                    "Relaciona abdomen, piel y estado mental para reconocer ascitis, hepatopatia y abdomen agudo.",
                    "Link the abdomen, skin, and mental status to recognize ascites, liver disease, and acute abdomen."),
                T(
                    "En abdomen no solo importan dolor y defensa. Tambien cuentan ictericia, circulacion colateral, ascitis, asterixis, desnutricion y signos cutaneos de hepatopatia cronica.",
                    "In abdominal semiology, pain and guarding are not enough. Jaundice, collateral veins, ascites, asterixis, malnutrition, and chronic liver disease stigmata also matter."),
                T(
                    "El patron es abdomen agudo, hipertension portal, falla hepatica, obstruccion o sangrado digestivo.",
                    "Is the pattern acute abdomen, portal hypertension, liver failure, obstruction, or gastrointestinal bleeding."),
                "gi_survey",
                Lines(
                    ("Inspecciona distension, cicatrices, venas visibles, coloracion y masa muscular.", "Inspect for distension, scars, visible veins, color changes, and muscle mass."),
                    ("Percute buscando ascitis, timpanismo o dolor localizado.", "Percuss looking for ascites, tympany, or localized pain."),
                    ("Relaciona abdomen con estado mental: asterixis y confusion pueden ser encefalopatia.", "Relate the abdomen to mental status: asterixis and confusion may mean encephalopathy."),
                    ("Describe si hay datos de irritacion peritoneal, obstruccion o sangrado activo.", "Describe whether there are signs of peritoneal irritation, obstruction, or active bleeding.")),
                Points(
                    ("Hipertension portal", "Portal hypertension", "Ascitis, esplenomegalia y circulacion colateral se refuerzan mutuamente.", "Ascites, splenomegaly, and collateral veins reinforce each other."),
                    ("Falla hepatica", "Liver failure", "Ictericia, asterixis, equimosis y fetor hepatico sugieren descompensacion.", "Jaundice, asterixis, bruising, and fetor hepaticus suggest decompensation."),
                    ("Irritacion peritoneal", "Peritoneal irritation", "Defensa involuntaria, rebote y dolor con el movimiento orientan a abdomen quirurgico.", "Involuntary guarding, rebound, and movement pain suggest a surgical abdomen."),
                    ("Obstruccion", "Obstruction", "Distension, vomito y ruidos agudos intermitentes forman un patron mecanico.", "Distension, vomiting, and intermittent high-pitched sounds form a mechanical pattern.")),
                Points(
                    ("Perla clave", "High-yield pearl", "La piel y el estado mental muchas veces hablan mas del higado que la palpacion aislada.", "Skin findings and mental status often tell you more about the liver than isolated palpation."),
                    ("Que aprender", "What to learn", "Relaciona siempre hallazgos abdominales con signos sistemicos.", "Always relate abdominal findings to systemic signs."),
                    ("Como memorizarlo", "Memory anchor", "Piel, panza, percusion y peritoneo: secuencia rapida del abdomen.", "Skin, belly, percussion, and peritoneum: quick abdominal sequence."),
                    ("Relacion con el quiz", "Quiz link", "Aunque este modulo no aparece directo en una pregunta, completa la base de semiologia general.", "Although this module is not asked directly, it completes the general semiology foundation.")),
                Lines(
                    ("Defensa involuntaria, rebote o abdomen rigido.", "Involuntary guarding, rebound, or rigid abdomen."),
                    ("Sangrado digestivo con inestabilidad hemodinamica.", "GI bleeding with hemodynamic instability."),
                    ("Confusion, ictericia y ascitis en el mismo paciente.", "Confusion, jaundice, and ascites in the same patient.")),
                Lines(
                    ("No limites la evaluacion abdominal a blando o depresible sin contexto sistemico.", "Do not limit abdominal assessment to soft or non-tender without systemic context."),
                    ("No olvides signos cutaneos y neurologicos de hepatopatia avanzada.", "Do not forget skin and neurologic signs of advanced liver disease."),
                    ("No confundas distension por obesidad con ascitis sin buscar matidez cambiante.", "Do not confuse obesity-related distension with ascites without checking for shifting dullness.")))
        ];
    }

    public static IReadOnlyList<MovementDisorderProfile> GetMovementProfiles()
    {
        return
        [
            new MovementDisorderProfile(
                "bradykinesia",
                T("Hipocinetico", "Hypokinetic"),
                T("Bradicinesia", "Bradykinesia"),
                T("Disminucion progresiva de velocidad y amplitud durante movimientos repetidos.", "Progressive reduction in speed and amplitude during repeated movement."),
                T("Hipomimia, menor parpadeo, gesticulacion pobre y tareas manuales cada vez mas pequenas.", "Hypomimia, reduced blinking, poor gesturing, and repetitive hand tasks that become smaller."),
                T("Haz finger tapping, abrir-cerrar manos o prono-supinacion durante 10 segundos y busca decremento.", "Perform finger tapping, hand opening-closing, or pronation-supination for 10 seconds and look for decrement."),
                T("No basta con decir que esta lento. La clave es que el movimiento se agota y se hace pequeno.", "It is not enough to say the patient is slow. The key is that movement fatigues and becomes small."),
                T("Ensenala como el signo central del parkinsonismo.", "Teach it as the central sign of parkinsonism."),
                "bradykinesia",
                Points(
                    ("Mejor contraste", "Best contrast", "La debilidad baja la fuerza; la bradicinesia baja la amplitud y la velocidad secuencial.", "Weakness lowers power; bradykinesia lowers sequential amplitude and speed."),
                    ("Donde verla", "Where to see it", "Aparece mejor en tareas repetitivas autoinducidas que en un unico movimiento aislado.", "It shows best in repetitive self-generated tasks rather than a single movement."),
                    ("Como decirlo", "How to phrase it", "Describe decremento, vacilacion y perdida de amplitud.", "Describe decrement, hesitation, and loss of amplitude.")),
                Lines(
                    ("Observa la mano dominante y luego compara el lado contrario.", "Observe the dominant hand and then compare with the contralateral side."),
                    ("Cuenta en voz alta las repeticiones para estandarizar la tarea.", "Count repetitions aloud to standardize the task."),
                    ("Fijate mas en la segunda mitad de la serie que en las primeras repeticiones.", "Pay more attention to the second half of the series than to the first repetitions."),
                    ("Anade marcha y braceo para buscar otros datos de parkinsonismo.", "Add gait and arm swing to look for other parkinsonian clues.")),
                Lines(
                    ("No la confundas con apatia o pobre cooperacion sin intentar tareas distintas.", "Do not confuse it with apathy or poor cooperation without trying different tasks."),
                    ("No diagnostiques bradicinesia por un movimiento unico lento.", "Do not diagnose bradykinesia from a single slow movement."),
                    ("No olvides el concepto de secuencia: el deterioro aparece conforme avanza la repeticion.", "Do not forget the sequence effect: deterioration appears as repetition continues."))),
            new MovementDisorderProfile(
                "rigidity",
                T("Hipocinetico", "Hypokinetic"),
                T("Rigidez", "Rigidity"),
                T("Aumento del tono pasivo constante durante todo el rango y no dependiente de velocidad.", "Constantly increased passive tone throughout the range, not velocity-dependent."),
                T("La extremidad se siente rigida aun cuando se moviliza lentamente; si se suma temblor puede haber rueda dentada.", "The limb feels stiff even when moved slowly; if tremor is superimposed there may be cogwheeling."),
                T("Moviliza muneca o codo con el paciente relajado y, si hace falta, agrega una maniobra activadora contralateral.", "Move the wrist or elbow with the patient relaxed and, if needed, add a contralateral activation maneuver."),
                T("La rigidez no depende de la velocidad; la espasticidad si.", "Rigidity does not depend on speed; spasticity does."),
                T("Ensenala con la frase: la misma resistencia en todo el trayecto.", "Teach it with the phrase: the same resistance all the way through."),
                "rigidity",
                Points(
                    ("Mejor contraste", "Best contrast", "La espasticidad empeora al movilizar rapido; la rigidez extrapiramidal no.", "Spasticity worsens with fast movement; extrapyramidal rigidity does not."),
                    ("Distribucion", "Distribution", "Puede ser axial y apendicular, favoreciendo postura flexionada.", "It can be axial and appendicular, favoring a flexed posture."),
                    ("Como decirlo", "How to phrase it", "Rigidez en tubo de plomo o en rueda dentada.", "Lead-pipe or cogwheel rigidity.")),
                Lines(
                    ("Pide relajacion completa antes de movilizar.", "Ask for complete relaxation before moving the limb."),
                    ("Explora cuello, hombro, codo y muneca si la rigidez es sutil.", "Assess neck, shoulder, elbow, and wrist if rigidity is subtle."),
                    ("Usa una maniobra activadora si el hallazgo es leve.", "Use an activating maneuver if the sign is mild."),
                    ("Relaciona el hallazgo con bradicinesia y marcha para hablar de parkinsonismo.", "Relate the sign to bradykinesia and gait before calling it parkinsonism.")),
                Lines(
                    ("No llames rigidez a la oposicion voluntaria del paciente.", "Do not call it rigidity when the patient is voluntarily resisting."),
                    ("No confundas rueda dentada con espasticidad en navaja.", "Do not confuse cogwheeling with clasp-knife spasticity."),
                    ("No olvides que la rigidez puede ser muy leve si no activas el lado contrario.", "Do not forget that rigidity can be subtle unless you activate the opposite side."))),
            new MovementDisorderProfile(
                "parkinson_tremor",
                T("Hipocinetico", "Hypokinetic"),
                T("Temblor parkinsoniano", "Parkinsonian tremor"),
                T("Temblor de reposo, usualmente asimetrico, con patron de contar monedas.", "Rest tremor, usually asymmetric, with a pill-rolling pattern."),
                T("Se hace mas visible cuando la mano descansa y suele disminuir con la accion voluntaria.", "It becomes more visible when the hand rests and often lessens with voluntary action."),
                T("Deja las manos apoyadas en el regazo y agrega una tarea cognitiva como contar hacia atras.", "Let the hands rest in the lap and add a cognitive task such as counting backward."),
                T("El estado de activacion manda: si es maximo en reposo, favorece parkinsonismo.", "Activation state rules: if it is maximal at rest, parkinsonism is favored."),
                T("Ensenalo nombrando primero la posicion: reposo, luego postura y luego accion.", "Teach it by naming the position first: rest, then posture, then action."),
                "parkinson_tremor",
                Points(
                    ("Mejor contraste", "Best contrast", "El temblor esencial es mas postural o cinetico y suele ser bilateral.", "Essential tremor is more postural or kinetic and usually bilateral."),
                    ("Acompanantes", "Associated clues", "Busca hipomimia, bradicinesia y menor braceo del mismo lado.", "Look for hypomimia, bradykinesia, and reduced arm swing on the same side."),
                    ("Como decirlo", "How to phrase it", "Temblor de reposo asimetrico con posible reemergencia al sostener postura.", "Asymmetric rest tremor with possible re-emergence on posture.")),
                Lines(
                    ("Observa ambas manos en reposo verdadero durante varios segundos.", "Observe both hands in true rest for several seconds."),
                    ("Anade distraccion mental si el temblor es intermitente.", "Add mental distraction if the tremor is intermittent."),
                    ("Comprueba si se reduce al alcanzar un objeto.", "Check whether it lessens when reaching for an object."),
                    ("Relaciona el temblor con lentitud y rigidez antes de etiquetarlo.", "Relate the tremor to slowness and rigidity before labeling it.")),
                Lines(
                    ("No diagnostiques temblor parkinsoniano por cualquier temblor de una mano.", "Do not diagnose parkinsonian tremor from any tremor in one hand."),
                    ("No olvides si el temblor aparece sentado, al mantener postura o al ejecutar un blanco.", "Do not forget whether the tremor appears sitting, with posture, or during a target-directed movement."),
                    ("No uses el termino pill-rolling sin describir si el fenomeno es de reposo.", "Do not use the term pill-rolling without describing that the phenomenon is a rest tremor."))),
            new MovementDisorderProfile(
                "essential_tremor",
                T("Hipercinetico", "Hyperkinetic"),
                T("Temblor esencial", "Essential tremor"),
                T("Temblor bilateral postural y cinetico, a menudo de manos, cabeza o voz.", "Bilateral postural and kinetic tremor, often involving hands, head, or voice."),
                T("Se acentua al extender brazos, escribir, dibujar espirales o servir agua.", "It increases when the arms are outstretched, during writing, spiral drawing, or pouring water."),
                T("Exploralo con postura sostenida, espiral de Arquimedes y maniobra dedo-nariz.", "Examine it with sustained posture, an Archimedes spiral, and finger-nose testing."),
                T("Es un temblor de accion sin verdadera bradicinesia ni rigidez.", "It is an action tremor without true bradykinesia or rigidity."),
                T("Ensenalo como un movimiento que aparece cuando el paciente intenta hacer algo.", "Teach it as a movement that appears when the patient tries to do something."),
                "essential_tremor",
                Points(
                    ("Mejor contraste", "Best contrast", "El temblor parkinsoniano predomina en reposo y suele ser asimetrico.", "Parkinsonian tremor predominates at rest and is often asymmetric."),
                    ("Acompanantes", "Associated clues", "Pregunta por historia familiar, mejoria con alcohol y compromiso de voz o cabeza.", "Ask about family history, alcohol responsiveness, and head or voice involvement."),
                    ("Como decirlo", "How to phrase it", "Temblor postural y cinetico sin decremento ni rigidez.", "Postural and kinetic tremor without decrement or rigidity.")),
                Lines(
                    ("Haz postura sostenida con brazos extendidos y dedos separados.", "Use sustained posture with arms outstretched and fingers spread."),
                    ("Pide dibujar una espiral y observa amplitud y regularidad.", "Ask for a spiral drawing and observe amplitude and regularity."),
                    ("Comprueba si tambien aparece al beber de un vaso.", "Check whether it also appears while drinking from a glass."),
                    ("Busca signos negativos: ausencia de hipomimia, rigidez y secuencia decreciente.", "Look for negative signs: no hypomimia, rigidity, or decremental sequence.")),
                Lines(
                    ("No llames esencial a un temblor sin explorar reposo.", "Do not call it essential tremor without examining rest."),
                    ("No confundas temblor de intencion cerebelosa con temblor esencial simple.", "Do not confuse cerebellar intention tremor with straightforward essential tremor."),
                    ("No pierdas los datos familiares y el compromiso de voz o cabeza.", "Do not miss family history or voice/head involvement."))),
            new MovementDisorderProfile(
                "chorea",
                T("Hipercinetico", "Hyperkinetic"),
                T("Corea", "Chorea"),
                T("Movimientos irregulares, impredecibles y no ritmicos que parecen fluir de una region a otra.", "Irregular, unpredictable, non-rhythmic movements that seem to flow from one region to another."),
                T("El paciente parece inquieto, con movimientos breves que se mezclan con gestos semivoluntarios.", "The patient looks restless, with brief movements blending into semipurposeful gestures."),
                T("Observa en reposo, durante conversacion y al pedir mantener una postura sostenida.", "Observe at rest, during conversation, and while asking for a sustained posture."),
                T("La corea no es ritmica como el temblor ni sostenida como la distonia.", "Chorea is not rhythmic like tremor and not sustained like dystonia."),
                T("Ensenala como desborde danzante e impredecible.", "Teach it as dance-like, unpredictable overflow."),
                "chorea",
                Points(
                    ("Mejor contraste", "Best contrast", "La distonia genera posturas mantenidas; la corea cambia todo el tiempo.", "Dystonia generates sustained postures; chorea keeps changing."),
                    ("Pistas utiles", "Useful clues", "Prension lechera, incapacidad de sostener la lengua afuera y movimientos que migran.", "Milkmaid grip, inability to sustain tongue protrusion, and migrating movements."),
                    ("Como decirlo", "How to phrase it", "Movimiento involuntario aleatorio, fluyente y no ritmico.", "Random, flowing, non-rhythmic involuntary movement.")),
                Lines(
                    ("No interrumpas la observacion; la corea se ve mejor durante la espontaneidad.", "Do not interrupt observation; chorea is seen best during spontaneous behavior."),
                    ("Pide protrusion lingual sostenida y observa si aparece impersistencia.", "Ask for sustained tongue protrusion and look for motor impersistence."),
                    ("Compara reposo y postura sostenida.", "Compare rest and sustained posture."),
                    ("Explora distribucion facial, distal y proximal.", "Examine facial, distal, and proximal distribution.")),
                Lines(
                    ("No la confundas con ansiedad motora o inquietud simple.", "Do not confuse it with anxious fidgeting or simple restlessness."),
                    ("No la describas como temblor si no hay oscilacion regular.", "Do not describe it as tremor when no regular oscillation exists."),
                    ("No olvides que puede esconderse dentro de gestos aparentemente voluntarios.", "Do not forget that it can hide inside apparently voluntary gestures."))),
            new MovementDisorderProfile(
                "dystonia",
                T("Hipercinetico", "Hyperkinetic"),
                T("Distonia", "Dystonia"),
                T("Contracciones sostenidas o intermitentes que producen posturas anormales y patrones repetitivos.", "Sustained or intermittent contractions producing abnormal postures and repetitive patterns."),
                T("Aparecen torsiones, desviaciones o posiciones repetidas; a veces un truco sensorial mejora el fenomeno.", "Twisting, deviations, or repeated abnormal positions appear; sometimes a sensory trick improves the phenomenon."),
                T("Observa en reposo y luego provoca con escritura, marcha, giro del cuello u otra tarea gatillo.", "Observe at rest and then provoke with writing, walking, neck turning, or another trigger task."),
                T("Postura mantenida y patron repetitivo inclinan el diagnostico hacia distonia.", "Sustained posture and repetitive pattern push the diagnosis toward dystonia."),
                T("Ensenala preguntando que postura intenta sostener el musculo una y otra vez.", "Teach it by asking what posture the muscle keeps trying to hold."),
                "dystonia",
                Points(
                    ("Mejor contraste", "Best contrast", "El mioclonus es un sacudon; la distonia sostiene y retuerce.", "Myoclonus is a jerk; dystonia sustains and twists."),
                    ("Truco sensorial", "Sensory trick", "Un toque ligero puede aliviar transitoriamente distonia cervical.", "A light touch can transiently relieve cervical dystonia."),
                    ("Como decirlo", "How to phrase it", "Postura anormal, torsional, repetitiva y a veces especifica de tarea.", "Abnormal, twisting, repetitive, sometimes task-specific posture.")),
                Lines(
                    ("Busca posiciones gatillo y tareas especificas.", "Look for triggering positions and task specificity."),
                    ("Observa si un gesto sensorial reduce el espasmo.", "Observe whether a sensory gesture reduces the spasm."),
                    ("Describe direccion del movimiento y grupo muscular dominante.", "Describe direction of movement and dominant muscle group."),
                    ("Comprueba si hay dolor o fatiga asociados.", "Check whether pain or fatigue are associated.")),
                Lines(
                    ("No la confundas con espasticidad si la postura es dinamica y repetitiva.", "Do not confuse it with spasticity when posture is dynamic and repetitive."),
                    ("No ignores la especificidad de tarea, especialmente en escritura y cuello.", "Do not ignore task specificity, especially in writing and the neck."),
                    ("No olvides preguntar por alivio con gestos sensoriales.", "Do not forget to ask about relief with sensory tricks."))),
            new MovementDisorderProfile(
                "myoclonus",
                T("Hipercinetico", "Hyperkinetic"),
                T("Mioclonus", "Myoclonus"),
                T("Sacudidas bruscas, breves y tipo descarga electrica, espontaneas o desencadenadas por estimulos.", "Sudden, brief, lightning-like jerks, spontaneous or stimulus-triggered."),
                T("El movimiento es muy rapido, fragmentario y puede ser multifocal.", "The movement is very fast, fragmentary, and may be multifocal."),
                T("Observa reposo y accion; luego verifica si tacto, sonido o movimiento provocan la sacudida.", "Observe rest and action, then check whether touch, sound, or movement triggers the jerk."),
                T("Es mucho mas rapido que la corea y no oscila regularmente como el temblor.", "It is much faster than chorea and does not oscillate regularly like tremor."),
                T("Ensenalo como una descarga, no como un movimiento fluyente.", "Teach it as a discharge, not as a flowing movement."),
                "myoclonus",
                Points(
                    ("Mejor contraste", "Best contrast", "El temblor oscila; el mioclonus interrumpe con una sacudida aislada.", "Tremor oscillates; myoclonus interrupts with a discrete jerk."),
                    ("Contexto clinico", "Clinical context", "Piensa en causas metabolicas, toxicas, epilepticas o degenerativas.", "Think of metabolic, toxic, epileptic, or degenerative causes."),
                    ("Como decirlo", "How to phrase it", "Sacudida involuntaria subita, breve y tipo shock.", "Sudden, brief, shock-like involuntary jerk.")),
                Lines(
                    ("Mira reposo y accion en varias partes del cuerpo.", "Look at rest and action in several body regions."),
                    ("Explora sensibilidad a estimulo acustico o tactil.", "Explore sensitivity to touch or sound."),
                    ("Describe si es focal, segmentario o multifocal.", "Describe whether it is focal, segmental, or multifocal."),
                    ("Relaciona el movimiento con estado metabolico o toxico del paciente.", "Relate the movement to the patient's metabolic or toxic context.")),
                Lines(
                    ("No lo llames temblor porque se mueve rapido; la regularidad es distinta.", "Do not call it tremor just because it moves fast; the regularity is different."),
                    ("No lo confundas con corea si la descarga es demasiado breve.", "Do not confuse it with chorea if the discharge is too brief."),
                    ("No pierdas el contexto sistemico: muchas causas no son primariamente del movimiento.", "Do not miss the systemic context: many causes are not primarily movement disorders."))),
            new MovementDisorderProfile(
                "parkinson_gait",
                T("Marcha", "Gait"),
                T("Marcha parkinsoniana", "Parkinsonian gait"),
                T("Postura flexionada, pasos cortos y arrastrados, menor braceo y dificultad para girar.", "Stooped posture, short shuffling steps, reduced arm swing, and difficulty turning."),
                T("Puede haber congelamiento al iniciar, en puertas o durante giros; el paciente gira en bloque.", "There may be freezing at initiation, at doorways, or during turns; the patient turns en bloc."),
                T("Haz caminar, detener, reiniciar, girar y agregar una tarea dual como contar hacia atras.", "Ask the patient to walk, stop, restart, turn, and perform a dual task such as counting backward."),
                T("Giros y transiciones suelen revelar mas que la marcha recta.", "Turns and transitions reveal more than straight walking."),
                T("Ensenala separando base, longitud del paso, braceo y calidad del giro.", "Teach it by separating base, stride length, arm swing, and turn quality."),
                "parkinson_gait",
                Points(
                    ("Mejor contraste", "Best contrast", "La marcha cerebelosa es amplia y tambaleante; la parkinsoniana es corta y festinante.", "Cerebellar gait is broad and staggering; parkinsonian gait is short and festinating."),
                    ("Reflejos posturales", "Postural reflexes", "Retropulsion o varios pasos correctivos sugieren inestabilidad postural.", "Retropulsion or several corrective steps suggest postural instability."),
                    ("Como decirlo", "How to phrase it", "Marcha de pasos cortos, braceo disminuido y congelamiento en giros.", "Short-step gait with reduced arm swing and freezing on turns.")),
                Lines(
                    ("Observa entrada, inicio, giro y detencion.", "Observe entry, initiation, turning, and stopping."),
                    ("Cuenta el numero de pasos para girar 180 grados.", "Count the number of steps needed to turn 180 degrees."),
                    ("Comprueba si una tarea dual empeora el patron.", "Check whether a dual task worsens the pattern."),
                    ("Busca reduccion asimetrica del braceo.", "Look for asymmetrically reduced arm swing.")),
                Lines(
                    ("No la describas solo como marcha lenta.", "Do not describe it only as slow gait."),
                    ("No olvides explorar giros y pasos correctivos.", "Do not forget to assess turns and corrective steps."),
                    ("No confundas inestabilidad cerebelosa con congelamiento parkinsoniano.", "Do not confuse cerebellar instability with parkinsonian freezing."))),
            new MovementDisorderProfile(
                "cerebellar_gait",
                T("Marcha", "Gait"),
                T("Marcha cerebelosa", "Cerebellar gait"),
                T("Marcha de base amplia, inestable, con oscilacion y mala marcha en tandem.", "Broad-based, unstable gait with veering and poor tandem walking."),
                T("La inestabilidad es por incoordinacion, no por pobreza de movimiento.", "Instability is due to incoordination, not paucity of movement."),
                T("Compara marcha habitual, tandem, giros y pruebas apendiculares como dedo-nariz o talon-rodilla.", "Compare usual gait, tandem gait, turns, and appendicular tests such as finger-nose or heel-shin."),
                T("La ataxia descompone el movimiento mas que enlentecerlo.", "Ataxia decomposes movement rather than slowing it."),
                T("Ensenala como un problema de coordinacion y balance, no de hipocinesia.", "Teach it as a coordination and balance problem, not a hypokinetic problem."),
                "cerebellar_gait",
                Points(
                    ("Mejor contraste", "Best contrast", "La marcha parkinsoniana reduce base y paso; la cerebelosa ensancha base y zigzaguea.", "Parkinsonian gait narrows the base and shortens steps; cerebellar gait widens the base and veers."),
                    ("Acompanantes", "Associated clues", "Dismetria, nistagmo y habla escandida fortalecen la sospecha cerebelosa.", "Dysmetria, nystagmus, and scanning speech strengthen a cerebellar interpretation."),
                    ("Como decirlo", "How to phrase it", "Marcha ataxica de base amplia con desviacion lateral.", "Broad-based ataxic gait with lateral veering.")),
                Lines(
                    ("Evalua marcha normal y en tandem.", "Assess normal gait and tandem gait."),
                    ("Busca si el paciente corrige con base amplia en cada paso.", "Look for wide-base correction with each step."),
                    ("Relaciona la marcha con dismetria de miembros.", "Relate gait findings to limb dysmetria."),
                    ("Observa si el giro es erratico mas que bloqueado.", "Observe whether turning is erratic rather than en bloc.")),
                Lines(
                    ("No la llames parkinsoniana porque el paciente camina despacio.", "Do not call it parkinsonian just because the patient walks slowly."),
                    ("No olvides la marcha en tandem: suele desenmascarar la ataxia.", "Do not forget tandem gait: it often unmasks ataxia."),
                    ("No ignores signos acompanantes como nistagmo o habla escandida.", "Do not ignore associated signs such as nystagmus or scanning speech.")))
        ];
    }

    public static IReadOnlyList<QuizQuestion> GetQuizQuestions()
    {
        return
        [
            new QuizQuestion(
                "q_bradykinesia",
                T("Trastornos del movimiento", "Movement disorders"),
                T("Un paciente hace finger tapping y, tras las primeras repeticiones, los movimientos se vuelven mas lentos y mas pequenos. Que signo estas viendo?", "A patient performs finger tapping and, after the first repetitions, movements become slower and smaller. What sign are you seeing?"),
                T("Busca decremento a lo largo de la secuencia, no solo lentitud aislada.", "Look for decrement along the sequence, not just isolated slowness."),
                "bradykinesia",
                "//movement",
                [
                    new QuizChoice(T("Bradicinesia", "Bradykinesia"), true, T("Correcto. La bradicinesia se reconoce por el decremento de velocidad y amplitud durante la repeticion.", "Correct. Bradykinesia is recognized by decrement in speed and amplitude during repetition.")),
                    new QuizChoice(T("Rigidez", "Rigidity"), false, T("La rigidez se palpa al movilizar pasivamente, no durante un tapping voluntario repetido.", "Rigidity is felt during passive movement, not during repetitive voluntary tapping.")),
                    new QuizChoice(T("Temblor esencial", "Essential tremor"), false, T("El temblor esencial es un fenomeno de accion y no produce secuencia decreciente.", "Essential tremor is an action phenomenon and does not produce a decrementing sequence."))
                ]),
            new QuizQuestion(
                "q_rigidity",
                T("Trastornos del movimiento", "Movement disorders"),
                T("Al flexionar y extender pasivamente la muneca, notas resistencia constante durante todo el rango, incluso con movilizacion lenta. Cual es el mejor termino?", "During passive wrist flexion and extension, you feel constant resistance through the whole range, even with slow movement. What is the best term?"),
                T("La pista es que la resistencia no depende de la velocidad.", "The clue is that resistance does not depend on velocity."),
                "rigidity",
                "//movement",
                [
                    new QuizChoice(T("Rigidez", "Rigidity"), true, T("Correcto. La rigidez es un aumento del tono pasivo independiente de la velocidad.", "Correct. Rigidity is increased passive tone independent of velocity.")),
                    new QuizChoice(T("Espasticidad", "Spasticity"), false, T("La espasticidad si depende de la velocidad y suele tener patron piramidal.", "Spasticity is velocity-dependent and usually has a pyramidal pattern.")),
                    new QuizChoice(T("Ataxia", "Ataxia"), false, T("La ataxia es incoordinacion, no aumento del tono pasivo.", "Ataxia is incoordination, not increased passive tone."))
                ]),
            new QuizQuestion(
                "q_parkinson_tremor",
                T("Trastornos del movimiento", "Movement disorders"),
                T("Una mano tiembla sobre todo cuando descansa en el regazo y mejora al alcanzar un objeto. Que patron favorece esto?", "One hand trembles mainly when resting in the lap and improves when reaching for an object. What pattern does this favor?"),
                T("Define siempre si el temblor es de reposo, postural o cinetico.", "Always define whether tremor is rest, postural, or kinetic."),
                "parkinson_tremor",
                "//movement",
                [
                    new QuizChoice(T("Temblor parkinsoniano", "Parkinsonian tremor"), true, T("Correcto. Un temblor asimetrico de reposo favorece parkinsonismo.", "Correct. An asymmetric rest tremor favors parkinsonism.")),
                    new QuizChoice(T("Temblor esencial", "Essential tremor"), false, T("El temblor esencial suele ser postural o cinetico, no predominantemente de reposo.", "Essential tremor is usually postural or kinetic, not predominantly a rest tremor.")),
                    new QuizChoice(T("Mioclonus", "Myoclonus"), false, T("El mioclonus es una sacudida breve y no una oscilacion ritmica de reposo.", "Myoclonus is a brief jerk, not a rhythmic rest oscillation."))
                ]),
            new QuizQuestion(
                "q_essential_tremor",
                T("Trastornos del movimiento", "Movement disorders"),
                T("Un paciente presenta temblor bilateral al sostener postura, servir agua y dibujar una espiral, pero no tiene rigidez ni decremento. Cual es el sindrome mas probable?", "A patient has bilateral tremor while holding posture, pouring water, and drawing a spiral, but there is no rigidity or decrement. What syndrome is most likely?"),
                T("Temblor de accion sin datos parkinsonianos orienta lejos de parkinsonismo.", "Action tremor without parkinsonian features points away from parkinsonism."),
                "essential_tremor",
                "//movement",
                [
                    new QuizChoice(T("Temblor esencial", "Essential tremor"), true, T("Correcto. Temblor postural o cinetico sin bradicinesia ni rigidez sugiere temblor esencial.", "Correct. Postural or kinetic tremor without bradykinesia or rigidity suggests essential tremor.")),
                    new QuizChoice(T("Temblor parkinsoniano", "Parkinsonian tremor"), false, T("El temblor parkinsoniano predomina en reposo.", "Parkinsonian tremor predominates at rest.")),
                    new QuizChoice(T("Distonia", "Dystonia"), false, T("La distonia produce posturas o torsiones anormales, no este patron clasico de temblor de accion.", "Dystonia produces abnormal postures or twisting, not this classic action tremor pattern."))
                ]),
            new QuizQuestion(
                "q_parkinson_gait",
                T("Marcha", "Gait"),
                T("Cual de las siguientes descripciones corresponde mejor a una marcha parkinsoniana?", "Which of the following descriptions best fits a parkinsonian gait?"),
                T("Piensa en base, longitud del paso, braceo y giros.", "Think of base, stride length, arm swing, and turning."),
                "parkinson_gait",
                "//movement",
                [
                    new QuizChoice(T("Postura flexionada, pasos cortos, braceo disminuido y congelamiento al girar", "Stooped posture, short steps, reduced arm swing, and freezing on turns"), true, T("Correcto. Esa combinacion es tipica de marcha parkinsoniana.", "Correct. That combination is typical of parkinsonian gait.")),
                    new QuizChoice(T("Base amplia, oscilacion lateral y mala marcha en tandem", "Broad base, lateral veering, and poor tandem gait"), false, T("Eso describe mejor una marcha cerebelosa.", "That better describes a cerebellar gait.")),
                    new QuizChoice(T("Marcha en steppage con caida del pie", "High-stepping gait with foot drop"), false, T("Eso orienta mas a neuropatia periferica o lesion peronea.", "That points more toward peripheral neuropathy or peroneal weakness."))
                ]),
            new QuizQuestion(
                "q_general_survey",
                T("Aspecto general", "General appearance"),
                T("Cual de estos hallazgos pertenece a la encuesta general antes de tocar al paciente?", "Which of these findings belongs to the general survey before touching the patient?"),
                T("La encuesta general es la evaluacion desde la puerta.", "The general survey is the doorway assessment."),
                "general_survey",
                "//library",
                [
                    new QuizChoice(T("Nivel de distres, postura, coloracion, hidratacion y adecuacion del vestido", "Level of distress, posture, color, hydration, and appropriateness of dress"), true, T("Correcto. Todo eso se observa antes del examen focal.", "Correct. All of that can be observed before the focused exam.")),
                    new QuizChoice(T("Reflejos osteotendinosos, Babinski y sensibilidad profunda", "Deep tendon reflexes, Babinski sign, and proprioception"), false, T("Eso pertenece a un examen neurologico focal.", "Those belong to a focused neurological exam.")),
                    new QuizChoice(T("Irradiacion de un soplo a carotidas", "Radiation of a murmur to the carotids"), false, T("Eso es un hallazgo de auscultacion cardiovascular, no de la encuesta general.", "That is a cardiovascular auscultation finding, not part of the general survey."))
                ]),
            new QuizQuestion(
                "q_respiratory_red_flag",
                T("Respiratorio", "Respiratory"),
                T("Paciente disneico que no logra hablar frases completas, usa musculos accesorios y tiene torax casi silencioso. Como debes interpretar este cuadro?", "A dyspneic patient cannot speak full sentences, uses accessory muscles, and has an almost silent chest. How should you interpret this picture?"),
                T("El silencio auscultatorio en un paciente fatigado es una bandera roja.", "A silent chest in a fatigued patient is a red flag."),
                "resp_pattern",
                "//library",
                [
                    new QuizChoice(T("Bandera roja respiratoria con posible falla ventilatoria inminente", "Respiratory red flag with possible impending ventilatory failure"), true, T("Correcto. El patron es de alta gravedad y requiere actuacion rapida.", "Correct. This is a high-severity pattern requiring prompt action.")),
                    new QuizChoice(T("Obstruccion leve estable", "Stable mild obstruction"), false, T("La incapacidad para hablar y el torax silencioso descartan gravedad leve.", "Inability to speak and a silent chest argue strongly against mild disease.")),
                    new QuizChoice(T("Derrame pleural aislado", "Isolated pleural effusion"), false, T("Aunque un derrame puede reducir ruidos, el conjunto aqui sugiere fatiga obstructiva grave.", "Although effusion can reduce sounds, the whole picture here suggests severe obstructive fatigue."))
                ]),
            new QuizQuestion(
                "q_chf_pattern",
                T("Cardiovascular", "Cardiovascular"),
                T("Yugulares elevadas, crepitos bibasales y edema bilateral en piernas orientan con mas fuerza a:", "Raised neck veins, bibasal crackles, and bilateral leg edema most strongly suggest:"),
                T("Agrupa los signos de congestion venosa y pulmonar en un mismo sindrome.", "Group venous and pulmonary congestion signs into a single syndrome."),
                "cardio_clues",
                "//library",
                [
                    new QuizChoice(T("Insuficiencia cardiaca congestiva", "Congestive heart failure"), true, T("Correcto. La triada semiologica clasica orienta a congestion cardiaca.", "Correct. The classic semiologic triad points to cardiac congestion.")),
                    new QuizChoice(T("Estenosis aortica aislada", "Isolated aortic stenosis"), false, T("La estenosis aortica puede causar falla, pero aqui el sindrome descrito es congestion global.", "Aortic stenosis can cause failure, but the syndrome described here is global congestion.")),
                    new QuizChoice(T("Neuropatia periferica", "Peripheral neuropathy"), false, T("La neuropatia no explica yugulares altas ni crepitos pulmonares.", "Peripheral neuropathy does not explain raised JVP or pulmonary crackles."))
                ])
        ];
    }

    private static string T(string spanish, string english)
    {
        return LocalizationService.Translate(spanish, english);
    }

    private static IReadOnlyList<string> Lines(params (string Spanish, string English)[] items)
    {
        return [.. items.Select(item => T(item.Spanish, item.English))];
    }

    private static IReadOnlyList<ClinicalPoint> Points(params (string SpanishTitle, string EnglishTitle, string SpanishDetail, string EnglishDetail)[] items)
    {
        return
        [
            .. items.Select(item => new ClinicalPoint(
                T(item.SpanishTitle, item.EnglishTitle),
                T(item.SpanishDetail, item.EnglishDetail)))
        ];
    }
}
