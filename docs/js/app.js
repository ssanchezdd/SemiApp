// ===== Navigation =====
const sidebar = document.getElementById('sidebar');
const overlay = document.getElementById('sidebarOverlay');
const menuToggle = document.getElementById('menuToggle');
const mainContent = document.getElementById('mainContent');

function navigateTo(sectionId) {
  // Hide all sections
  document.querySelectorAll('.section').forEach(s => s.classList.remove('active'));
  // Show target
  const target = document.getElementById(sectionId);
  if (target) {
    target.classList.add('active');
    mainContent.scrollTop = 0;
    window.scrollTo({ top: 0, behavior: 'instant' });
  }
  // Update sidebar
  document.querySelectorAll('.sidebar a').forEach(a => {
    a.classList.toggle('active', a.dataset.section === sectionId);
  });
  // Close mobile sidebar
  closeSidebar();
  // Render quiz if needed
  if (sectionId === 'autoevaluación' && !quizRendered) renderQuiz();
}

// Sidebar links
document.querySelectorAll('.sidebar a[data-section]').forEach(link => {
  link.addEventListener('click', e => {
    e.preventDefault();
    navigateTo(link.dataset.section);
  });
});

// Card clicks (already handled vía onclick in HTML, but also expose globally)
window.navigateTo = navigateTo;

// Mobile menu
function openSidebar() { sidebar.classList.add('open'); overlay.classList.add('show'); }
function closeSidebar() { sidebar.classList.remove('open'); overlay.classList.remove('show'); }
menuToggle.addEventListener('click', () => sidebar.classList.contains('open') ? closeSidebar() : openSidebar());
overlay.addEventListener('click', closeSidebar);

// Scroll to top
const scrollTopBtn = document.getElementById('scrollTop');
window.addEventListener('scroll', () => {
  scrollTopBtn.classList.toggle('visible', window.scrollY > 400);
});
scrollTopBtn.addEventListener('click', () => window.scrollTo({ top: 0, behavior: 'smooth' }));

// ===== Quiz =====
const quizData = [
  {
    question: 'Cual es la definición de "ojo clínico"?',
    hint: 'Piensa en la habilidad de observación de los maestros de la medicina.',
    options: [
      { text: 'Aptitud de un médico para diagnósticos rápidos y certeros basados en la observación', correct: true, explanation: 'Correcto. El ojo clínico es la capacidad de captar signos y síntomas mediante la observación detallada desde el primer contacto.' },
      { text: 'La capacidad de realizar una buena auscultación cardiaca', correct: false, explanation: 'La auscultación es parte del examen físico, pero el ojo clínico se refiere a la observación global del paciente.' },
      { text: 'El uso de tecnología moderna para el diagnóstico', correct: false, explanation: 'El ojo clínico es previo a la tecnología; se basa en la observación directa del paciente.' }
    ]
  },
  {
    question: 'En el tipo constitucional longilineo, el ángulo epigástrico es:',
    hint: 'Recuerda las tres clasificaciones: longilineo, brevilineo, normolineo.',
    options: [
      { text: 'Agudo', correct: true, explanation: 'Correcto. En el longilineo el ángulo epigástrico es agudo, el tórax es estrecho y alargado.' },
      { text: 'Recto', correct: false, explanation: 'El ángulo recto corresponde al normolineo o atletico.' },
      { text: 'Obtuso', correct: false, explanation: 'El ángulo obtuso corresponde al brevilineo o picnico.' }
    ]
  },
  {
    question: 'La facies de "luna llena", pletórica, con coloración rojo cianótica y boca de pescado corresponde a:',
    hint: 'Piensa en patologías endocrinas que causan redistribución de la grasa.',
    options: [
      { text: 'Síndrome de Cushing', correct: true, explanation: 'Correcto. La cara de luna llena con pletórica y acné es característica del síndrome de Cushing.' },
      { text: 'Acromegalia', correct: false, explanation: 'La acromegalia se caracteriza por aumento del tamaño del cráneo, maxilar inferior y extremidades.' },
      { text: 'Mixedema', correct: false, explanation: 'El mixedema presenta cara abotagada, párpados edematosos, piel seca y áspera.' }
    ]
  },
  {
    question: 'La cianosis periférica se diferencia de la central porque:',
    hint: 'Piensa en donde se manifiesta cada tipo de cianosis.',
    options: [
      { text: 'No compromete las mucosas', correct: true, explanation: 'Correcto. La cianosis periférica se evidencia solo en partes distales, nariz, orejas y región peribucal, pero NO compromete las mucosas.' },
      { text: 'Siempre se acompaña de ictericia', correct: false, explanation: 'Cianosis e ictericia son signos independientes con mecanismos fisiopatológicos diferentes.' },
      { text: 'Solo aparece en niños', correct: false, explanation: 'Ambos tipos de cianosis pueden presentarse a cualquier edad.' }
    ]
  },
  {
    question: 'Los movimientos involuntarios amplios, desordenados, inesperados, arrítmicos y sin finalidad aparente se denominan:',
    hint: 'Piensa en el significado etimologico del término (danza).',
    options: [
      { text: 'Corea', correct: true, explanation: 'Correcto. La corea se caracteriza por movimientos amplios, desordenados, arrítmicos, que comprometen cara y extremidades.' },
      { text: 'Atetosis', correct: false, explanation: 'La atetosis son movimientos lentos, estereotipados y reptiformes de los segmentos distales.' },
      { text: 'Fasciculaciones', correct: false, explanation: 'Las fasciculaciones son contracciones breves y arrítmicas de un haz muscular aislado.' }
    ]
  },
  {
    question: 'La posición genupectoral o de plegaria mahometana es adoptada por pacientes con:',
    hint: 'Piensa en que patología cardiaca mejora al inclinarse hacia adelante.',
    options: [
      { text: 'Pericarditis con derrame', correct: true, explanation: 'Correcto. En la pericarditis con derrame, esta posición alivia la compresión cardiaca al alejar el líquido del corazón.' },
      { text: 'Fractura de columna', correct: false, explanation: 'En fractura de columna el paciente adopta decubito dorsal (supino).' },
      { text: 'Distensión abdominal', correct: false, explanation: 'La distensión abdominal se relaciona más con decubito dorsal.' }
    ]
  },
  {
    question: 'La marcha cerebelosa se diferencia de la marcha atáxica (tabética) en que:',
    hint: 'Piensa en la prueba de Romberg y el efecto de cerrar los ojos.',
    options: [
      { text: 'El desequilibrio no empeora al cerrar los ojos', correct: true, explanation: 'Correcto. En la marcha cerebelosa el desequilibrio no se incrementa al cerrar los ojos, a diferencia de la marcha tabética que empeora con la oscuridad.' },
      { text: 'Se presenta con pasos cortos y arrastrados', correct: false, explanation: 'Los pasos cortos y arrastrados son más típicos de la marcha parkinsoniana.' },
      { text: 'El paciente mantiene los brazos pegados al cuerpo', correct: false, explanation: 'En la marcha cerebelosa los brazos estan extendidos y en abducción para mejorar el equilibrio.' }
    ]
  },
  {
    question: 'Un paciente que responde solo ante estímulos vigorosos se encuentra en estado de:',
    hint: 'Revisa los cuatro niveles de conciencia.',
    options: [
      { text: 'Estupor', correct: true, explanation: 'Correcto. El estupor es el estado en que el paciente parece dormido y despierta solamente ante estímulos vigorosos.' },
      { text: 'Somnolencia', correct: false, explanation: 'En la somnolencia el paciente tiene un sueño ligero y responde a estímulos comunes por periodos cortos.' },
      { text: 'Coma superficial', correct: false, explanation: 'En el coma superficial el paciente no despierta pero responde a estímulos de dolor profundo.' }
    ]
  },
  {
    question: 'La distribución centrípeta de la grasa (tronco obeso con extremidades normales) es característica de:',
    hint: 'Diferencia entre obesidad exógena y endógena.',
    options: [
      { text: 'Obesidad endógena (ejemplo: enfermedad de Cushing)', correct: true, explanation: 'Correcto. En la obesidad endógena la grasa se acumula centralmente en el tronco mientras las extremidades permanecen normales.' },
      { text: 'Obesidad tipo ginecoide', correct: false, explanation: 'La obesidad ginecoide acumula grasa en la mitad inferior del cuerpo (caderas, muslos).' },
      { text: 'Desnutrición con edema', correct: false, explanation: 'La desnutrición presenta disminución de la masa muscular, no acumulación de grasa.' }
    ]
  },
  {
    question: 'El olor a ajo en un paciente intoxicado sugiere exposición a:',
    hint: 'Piensa en sustancias tóxicas comunes en nuestro medio.',
    options: [
      { text: 'Organofosforados', correct: true, explanation: 'Correcto. La intoxicación por organofosforados produce un olor característico a ajo.' },
      { text: 'Hipoclorito de sodio', correct: false, explanation: 'La intoxicación por hipoclorito de sodio produce olor a límpido (cloro).' },
      { text: 'Hidrocarburos', correct: false, explanation: 'La intoxicación por hidrocarburos produce olor a gasolina.' }
    ]
  },
  {
    question: 'La tos quintosa con paroxismos, espiraciones violentas y "reprise" inspiratoria es típica de:',
    hint: 'Piensa en enfermedades infecciosas respiratorias de la infancia.',
    options: [
      { text: 'Tos ferina (pertussis)', correct: true, explanation: 'Correcto. La tos quintosa con reprise es la presentación clásica de la tos ferina.' },
      { text: 'Síndrome mediastinico', correct: false, explanation: 'El síndrome mediastinico produce tos coqueluchoide (similar pero sin reprise ni expectoración hialina).' },
      { text: 'Pleuritis', correct: false, explanation: 'La pleuritis produce tos seca, incompleta y dolorosa.' }
    ]
  },
  {
    question: 'Que hallazgo pertenece a la encuesta general (antes de tocar al paciente)?',
    hint: 'La encuesta general es la evaluación visual desde la puerta.',
    options: [
      { text: 'Nivel de alerta, postura, coloración, hidratación y vestido', correct: true, explanation: 'Correcto. Todo eso se observa antes del examen físico focal, desde el primer contacto visual con el paciente.' },
      { text: 'Reflejos osteotendinosos y signo de Babinski', correct: false, explanation: 'Los reflejos pertenecen al examen neurológico focal, que requiere contacto con el paciente.' },
      { text: 'Irradiación de un soplo cardíaco', correct: false, explanation: 'La auscultación cardiaca es parte del examen físico, no de la inspección general.' }
    ]
  },
  {
    question: 'La marcha parkinsoniana se caracteriza por todos EXCEPTO:',
    hint: 'Piensa en base de sustentación, braceo y tipo de pasos.',
    options: [
      { text: 'Base de sustentación amplia con desviación lateral', correct: true, explanation: 'Correcto. La base amplia y desviación lateral son características de la marcha cerebelosa, no de la parkinsoniana.' },
      { text: 'Pasos cortos y arrastrados', correct: false, explanation: 'Los pasos cortos y arrastrados SI son característicos de la marcha parkinsoniana.' },
      { text: 'Disminución del braceo y dificultad para girar', correct: false, explanation: 'La disminución del braceo y la dificultad para girar SI son característicos de la marcha parkinsoniana.' }
    ]
  },
  {
    question: 'La orientación en persona evalua:',
    hint: 'Cada tipo de orientación evalua un aspecto diferente.',
    options: [
      { text: 'Conocimiento de su nombre, profesión e historia personal', correct: true, explanation: 'Correcto. La orientación en persona evalua si el paciente se reconoce a si mismo y conoce sus datos basicos.' },
      { text: 'Ubicación del lugar, ciudad y país', correct: false, explanation: 'Eso corresponde a la orientación en espacio.' },
      { text: 'Año, mes, día y hora aproximada', correct: false, explanation: 'Eso corresponde a la orientación en tiempo.' }
    ]
  },
  {
    question: 'Un paciente con nariz afilada, ojos y sienes hundidos, orejas contraidas y color plomizo presenta facies:',
    hint: 'Piensa en el aspecto de un paciente gravemente enfermo o moribundo.',
    options: [
      { text: 'Hipocrática', correct: true, explanation: 'Correcto. La facies hipocratica se presenta en pacientes moribundos y con deshidratación.' },
      { text: 'Renal', correct: false, explanation: 'La facies renal presenta edema periocular y piel pálida, no hundimiento de ojos ni color plomizo.' },
      { text: 'Leonina', correct: false, explanation: 'La facies leonina presenta infiltración subcutanea con nariz aplanada y ensanchada, típica de la lepra.' }
    ]
  }
];

let quizRendered = false;
let currentQuestion = 0;
let score = 0;
let answered = new Array(quizData.length).fill(false);
let shuffledOptions = [];

function shuffle(arr) {
  const a = arr.slice();
  for (let i = a.length - 1; i > 0; i--) {
    const j = Math.floor(Math.random() * (i + 1));
    [a[i], a[j]] = [a[j], a[i]];
  }
  return a;
}

function renderQuiz() {
  quizRendered = true;
  currentQuestion = 0;
  score = 0;
  answered = new Array(quizData.length).fill(false);
  shuffledOptions = quizData.map(q => shuffle(q.options));
  showQuestion(0);
}

function showQuestion(idx) {
  currentQuestion = idx;
  const q = quizData[idx];
  const opts = shuffledOptions[idx];
  const container = document.getElementById('quizContainer');
  const pct = Math.round((idx / quizData.length) * 100);

  let optionsHtml = opts.map((o, i) =>
    `<div class="quiz-option" data-idx="${i}" onclick="selectOption(this, ${i})">${o.text}</div>`
  ).join('');

  container.innerHTML = `
    <div class="quiz-progress">
      <span>Pregunta ${idx + 1} de ${quizData.length}</span>
      <div class="quiz-progress-bar"><div class="quiz-progress-fill" style="width:${pct}%"></div></div>
      <span>${score} correctas</span>
    </div>
    <div class="quiz-question">
      <h3>${q.question}</h3>
      <div class="quiz-hint">${q.hint}</div>
      <div class="quiz-options" id="quizOptions">${optionsHtml}</div>
      <div class="quiz-feedback" id="quizFeedback"></div>
    </div>
    <div class="quiz-nav">
      ${idx > 0 ? '<button class="btn btn-outline" onclick="showQuestion(' + (idx - 1) + ')">Anterior</button>' : ''}
      <button class="btn btn-primary" id="nextBtn" onclick="nextQuestion()" style="display:none">
        ${idx < quizData.length - 1 ? 'Siguiente' : 'Ver resultados'}
      </button>
    </div>
  `;
}

function selectOption(el, optIdx) {
  if (answered[currentQuestion]) return;
  answered[currentQuestion] = true;

  const opts = shuffledOptions[currentQuestion];
  const options = document.querySelectorAll('.quiz-option');
  const feedback = document.getElementById('quizFeedback');
  const nextBtn = document.getElementById('nextBtn');

  options.forEach((opt, i) => {
    opt.classList.add('disabled');
    if (opts[i].correct) opt.classList.add('correct');
  });

  if (opts[optIdx].correct) {
    el.classList.add('correct');
    score++;
    feedback.className = 'quiz-feedback show correct';
  } else {
    el.classList.add('incorrect');
    feedback.className = 'quiz-feedback show incorrect';
  }
  feedback.textContent = opts[optIdx].explanation;
  nextBtn.style.display = '';
}

function nextQuestion() {
  if (currentQuestion < quizData.length - 1) {
    showQuestion(currentQuestion + 1);
  } else {
    showResults();
  }
}

function showResults() {
  const container = document.getElementById('quizContainer');
  const pct = Math.round((score / quizData.length) * 100);
  let message;
  if (pct >= 90) message = 'Excelente dominio de la apariencia general.';
  else if (pct >= 70) message = 'Buen desempeño. Repasa los temas donde tuviste dificultad.';
  else if (pct >= 50) message = 'Desempeño aceptable. Te recomendamos revisar el contenido nuevamente.';
  else message = 'Necesitas repasar el contenido. Revisa cada parámetro y vuelve a intentarlo.';

  container.innerHTML = `
    <div class="quiz-results">
      <div class="score">${score}/${quizData.length}</div>
      <div class="score-label">${pct}% de respuestas correctas</div>
      <p class="score-message">${message}</p>
      <button class="btn btn-primary" onclick="renderQuiz()">Reiniciar evaluación</button>
      <button class="btn btn-outline" style="margin-left:.5rem" onclick="navigateTo('inicio')">Volver al inicio</button>
    </div>
  `;
}

// Expose functions globally
window.selectOption = selectOption;
window.nextQuestion = nextQuestion;
window.renderQuiz = renderQuiz;

// ===== Lightbox =====
function openLightbox(el) {
  const img = el.querySelector('img');
  if (!img) return;
  const lb = document.getElementById('lightbox');
  const lbImg = document.getElementById('lightboxImg');
  lbImg.src = img.src;
  lbImg.alt = img.alt;
  lb.classList.add('show');
}
function closeLightbox() {
  document.getElementById('lightbox').classList.remove('show');
}
document.addEventListener('keydown', e => { if (e.key === 'Escape') closeLightbox(); });
window.openLightbox = openLightbox;
window.closeLightbox = closeLightbox;

// ===== Hash-based navigation =====
function handleHash() {
  const hash = window.location.hash.replace('#', '');
  if (hash) navigateTo(hash);
}
window.addEventListener('hashchange', handleHash);
if (window.location.hash) handleHash();

// ===== Prev/Next section nav buttons =====
function buildSectionNav() {
  const links = Array.from(document.querySelectorAll('.sidebar a[data-section]'));
  const seq = links.map(a => {
    const icon = a.querySelector('.nav-icon');
    const label = a.textContent.replace(icon ? icon.textContent : '', '').trim();
    return { id: a.dataset.section, label };
  });
  document.querySelectorAll('.section').forEach(section => {
    const i = seq.findIndex(s => s.id === section.id);
    if (i === -1) return;
    const prev = i > 0 ? seq[i - 1] : null;
    const next = i < seq.length - 1 ? seq[i + 1] : null;
    const nav = document.createElement('div');
    nav.className = 'section-nav';
    const prevHtml = prev
      ? `<button class="section-nav-btn prev" onclick="navigateTo('${prev.id}')" aria-label="Sección anterior: ${prev.label}"><span class="arrow">&larr;</span><span class="label"><span class="small">Anterior</span><span class="title">${prev.label}</span></span></button>`
      : '<span class="section-nav-spacer"></span>';
    const nextHtml = next
      ? `<button class="section-nav-btn next" onclick="navigateTo('${next.id}')" aria-label="Sección siguiente: ${next.label}"><span class="label"><span class="small">Siguiente</span><span class="title">${next.label}</span></span><span class="arrow">&rarr;</span></button>`
      : '<span class="section-nav-spacer"></span>';
    nav.innerHTML = prevHtml + nextHtml;
    section.appendChild(nav);
  });
}
document.addEventListener('DOMContentLoaded', buildSectionNav);
