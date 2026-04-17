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
  if (sectionId === 'autoevaluacion' && !quizRendered) renderQuiz();
}

// Sidebar links
document.querySelectorAll('.sidebar a[data-section]').forEach(link => {
  link.addEventListener('click', e => {
    e.preventDefault();
    navigateTo(link.dataset.section);
  });
});

// Card clicks (already handled via onclick in HTML, but also expose globally)
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
    question: 'Cual es la definicion de "ojo clinico"?',
    hint: 'Piensa en la habilidad de observacion de los maestros de la medicina.',
    options: [
      { text: 'Aptitud de un medico para diagnosticos rapidos y certeros basados en la observacion', correct: true, explanation: 'Correcto. El ojo clinico es la capacidad de captar signos y sintomas mediante la observacion detallada desde el primer contacto.' },
      { text: 'La capacidad de realizar una buena auscultacion cardiaca', correct: false, explanation: 'La auscultacion es parte del examen fisico, pero el ojo clinico se refiere a la observacion global del paciente.' },
      { text: 'El uso de tecnologia moderna para el diagnostico', correct: false, explanation: 'El ojo clinico es previo a la tecnologia; se basa en la observacion directa del paciente.' }
    ]
  },
  {
    question: 'En el tipo constitucional longilineo, el angulo epigastrico es:',
    hint: 'Recuerda las tres clasificaciones: longilineo, brevilineo, normolineo.',
    options: [
      { text: 'Agudo', correct: true, explanation: 'Correcto. En el longilineo el angulo epigastrico es agudo, el torax es estrecho y alargado.' },
      { text: 'Recto', correct: false, explanation: 'El angulo recto corresponde al normolineo o atletico.' },
      { text: 'Obtuso', correct: false, explanation: 'El angulo obtuso corresponde al brevilineo o picnico.' }
    ]
  },
  {
    question: 'La facies de "luna llena", pletorica, con coloracion rojo cianotica y boca de pescado corresponde a:',
    hint: 'Piensa en patologias endocrinas que causan redistribucion de la grasa.',
    options: [
      { text: 'Sindrome de Cushing', correct: true, explanation: 'Correcto. La cara de luna llena con pletorica y acne es caracteristica del sindrome de Cushing.' },
      { text: 'Acromegalia', correct: false, explanation: 'La acromegalia se caracteriza por aumento del tamaño del craneo, maxilar inferior y extremidades.' },
      { text: 'Mixedema', correct: false, explanation: 'El mixedema presenta cara abotagada, parpados edematosos, piel seca y aspera.' }
    ]
  },
  {
    question: 'La cianosis periferica se diferencia de la central porque:',
    hint: 'Piensa en donde se manifiesta cada tipo de cianosis.',
    options: [
      { text: 'No compromete las mucosas', correct: true, explanation: 'Correcto. La cianosis periferica se evidencia solo en partes distales, nariz, orejas y region peribucal, pero NO compromete las mucosas.' },
      { text: 'Siempre se acompaña de ictericia', correct: false, explanation: 'Cianosis e ictericia son signos independientes con mecanismos fisiopatologicos diferentes.' },
      { text: 'Solo aparece en niños', correct: false, explanation: 'Ambos tipos de cianosis pueden presentarse a cualquier edad.' }
    ]
  },
  {
    question: 'Los movimientos involuntarios amplios, desordenados, inesperados, arritmicos y sin finalidad aparente se denominan:',
    hint: 'Piensa en el significado etimologico del termino (danza).',
    options: [
      { text: 'Corea', correct: true, explanation: 'Correcto. La corea se caracteriza por movimientos amplios, desordenados, arritmicos, que comprometen cara y extremidades.' },
      { text: 'Atetosis', correct: false, explanation: 'La atetosis son movimientos lentos, estereotipados y reptiformes de los segmentos distales.' },
      { text: 'Fasciculaciones', correct: false, explanation: 'Las fasciculaciones son contracciones breves y arritmicas de un haz muscular aislado.' }
    ]
  },
  {
    question: 'La posicion genupectoral o de plegaria mahometana es adoptada por pacientes con:',
    hint: 'Piensa en que patologia cardiaca mejora al inclinarse hacia adelante.',
    options: [
      { text: 'Pericarditis con derrame', correct: true, explanation: 'Correcto. En la pericarditis con derrame, esta posicion alivia la compresion cardiaca al alejar el liquido del corazon.' },
      { text: 'Fractura de columna', correct: false, explanation: 'En fractura de columna el paciente adopta decubito dorsal (supino).' },
      { text: 'Distension abdominal', correct: false, explanation: 'La distension abdominal se relaciona mas con decubito dorsal.' }
    ]
  },
  {
    question: 'La marcha cerebelosa se diferencia de la marcha ataxica (tabetica) en que:',
    hint: 'Piensa en la prueba de Romberg y el efecto de cerrar los ojos.',
    options: [
      { text: 'El desequilibrio no empeora al cerrar los ojos', correct: true, explanation: 'Correcto. En la marcha cerebelosa el desequilibrio no se incrementa al cerrar los ojos, a diferencia de la marcha tabetica que empeora con la oscuridad.' },
      { text: 'Se presenta con pasos cortos y arrastrados', correct: false, explanation: 'Los pasos cortos y arrastrados son mas tipicos de la marcha parkinsoniana.' },
      { text: 'El paciente mantiene los brazos pegados al cuerpo', correct: false, explanation: 'En la marcha cerebelosa los brazos estan extendidos y en abduccion para mejorar el equilibrio.' }
    ]
  },
  {
    question: 'Un paciente que responde solo ante estimulos vigorosos se encuentra en estado de:',
    hint: 'Revisa los cuatro niveles de conciencia.',
    options: [
      { text: 'Estupor', correct: true, explanation: 'Correcto. El estupor es el estado en que el paciente parece dormido y despierta solamente ante estimulos vigorosos.' },
      { text: 'Somnolencia', correct: false, explanation: 'En la somnolencia el paciente tiene un sueño ligero y responde a estimulos comunes por periodos cortos.' },
      { text: 'Coma superficial', correct: false, explanation: 'En el coma superficial el paciente no despierta pero responde a estimulos de dolor profundo.' }
    ]
  },
  {
    question: 'La distribucion centripeta de la grasa (tronco obeso con extremidades normales) es caracteristica de:',
    hint: 'Diferencia entre obesidad exogena y endogena.',
    options: [
      { text: 'Obesidad endogena (ejemplo: enfermedad de Cushing)', correct: true, explanation: 'Correcto. En la obesidad endogena la grasa se acumula centralmente en el tronco mientras las extremidades permanecen normales.' },
      { text: 'Obesidad tipo ginecoide', correct: false, explanation: 'La obesidad ginecoide acumula grasa en la mitad inferior del cuerpo (caderas, muslos).' },
      { text: 'Desnutricion con edema', correct: false, explanation: 'La desnutricion presenta disminucion de la masa muscular, no acumulacion de grasa.' }
    ]
  },
  {
    question: 'El olor a ajo en un paciente intoxicado sugiere exposicion a:',
    hint: 'Piensa en sustancias toxicas comunes en nuestro medio.',
    options: [
      { text: 'Organofosforados', correct: true, explanation: 'Correcto. La intoxicacion por organofosforados produce un olor caracteristico a ajo.' },
      { text: 'Hipoclorito de sodio', correct: false, explanation: 'La intoxicacion por hipoclorito de sodio produce olor a limpido (cloro).' },
      { text: 'Hidrocarburos', correct: false, explanation: 'La intoxicacion por hidrocarburos produce olor a gasolina.' }
    ]
  },
  {
    question: 'La tos quintosa con paroxismos, espiraciones violentas y "reprise" inspiratoria es tipica de:',
    hint: 'Piensa en enfermedades infecciosas respiratorias de la infancia.',
    options: [
      { text: 'Tos ferina (pertussis)', correct: true, explanation: 'Correcto. La tos quintosa con reprise es la presentacion clasica de la tos ferina.' },
      { text: 'Sindrome mediastinico', correct: false, explanation: 'El sindrome mediastinico produce tos coqueluchoide (similar pero sin reprise ni expectoracion hialina).' },
      { text: 'Pleuritis', correct: false, explanation: 'La pleuritis produce tos seca, incompleta y dolorosa.' }
    ]
  },
  {
    question: 'Que hallazgo pertenece a la encuesta general (antes de tocar al paciente)?',
    hint: 'La encuesta general es la evaluacion visual desde la puerta.',
    options: [
      { text: 'Nivel de alerta, postura, coloracion, hidratacion y vestido', correct: true, explanation: 'Correcto. Todo eso se observa antes del examen fisico focal, desde el primer contacto visual con el paciente.' },
      { text: 'Reflejos osteotendinosos y signo de Babinski', correct: false, explanation: 'Los reflejos pertenecen al examen neurologico focal, que requiere contacto con el paciente.' },
      { text: 'Irradiacion de un soplo cardiaco', correct: false, explanation: 'La auscultacion cardiaca es parte del examen fisico, no de la inspeccion general.' }
    ]
  },
  {
    question: 'La marcha parkinsoniana se caracteriza por todos EXCEPTO:',
    hint: 'Piensa en base de sustentacion, braceo y tipo de pasos.',
    options: [
      { text: 'Base de sustentacion amplia con desviacion lateral', correct: true, explanation: 'Correcto. La base amplia y desviacion lateral son caracteristicas de la marcha cerebelosa, no de la parkinsoniana.' },
      { text: 'Pasos cortos y arrastrados', correct: false, explanation: 'Los pasos cortos y arrastrados SI son caracteristicos de la marcha parkinsoniana.' },
      { text: 'Disminucion del braceo y dificultad para girar', correct: false, explanation: 'La disminucion del braceo y la dificultad para girar SI son caracteristicos de la marcha parkinsoniana.' }
    ]
  },
  {
    question: 'La orientacion en persona evalua:',
    hint: 'Cada tipo de orientacion evalua un aspecto diferente.',
    options: [
      { text: 'Conocimiento de su nombre, profesion e historia personal', correct: true, explanation: 'Correcto. La orientacion en persona evalua si el paciente se reconoce a si mismo y conoce sus datos basicos.' },
      { text: 'Ubicacion del lugar, ciudad y pais', correct: false, explanation: 'Eso corresponde a la orientacion en espacio.' },
      { text: 'Año, mes, dia y hora aproximada', correct: false, explanation: 'Eso corresponde a la orientacion en tiempo.' }
    ]
  },
  {
    question: 'Un paciente con nariz afilada, ojos y sienes hundidos, orejas contraidas y color plomizo presenta facies:',
    hint: 'Piensa en el aspecto de un paciente gravemente enfermo o moribundo.',
    options: [
      { text: 'Hipocratica', correct: true, explanation: 'Correcto. La facies hipocratica se presenta en pacientes moribundos y con deshidratacion.' },
      { text: 'Renal', correct: false, explanation: 'La facies renal presenta edema periocular y piel palida, no hundimiento de ojos ni color plomizo.' },
      { text: 'Leonina', correct: false, explanation: 'La facies leonina presenta infiltracion subcutanea con nariz aplanada y ensanchada, tipica de la lepra.' }
    ]
  }
];

let quizRendered = false;
let currentQuestion = 0;
let score = 0;
let answered = new Array(quizData.length).fill(false);

function renderQuiz() {
  quizRendered = true;
  currentQuestion = 0;
  score = 0;
  answered = new Array(quizData.length).fill(false);
  showQuestion(0);
}

function showQuestion(idx) {
  currentQuestion = idx;
  const q = quizData[idx];
  const container = document.getElementById('quizContainer');
  const pct = Math.round((idx / quizData.length) * 100);

  let optionsHtml = q.options.map((o, i) =>
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

  const q = quizData[currentQuestion];
  const options = document.querySelectorAll('.quiz-option');
  const feedback = document.getElementById('quizFeedback');
  const nextBtn = document.getElementById('nextBtn');

  options.forEach((opt, i) => {
    opt.classList.add('disabled');
    if (q.options[i].correct) opt.classList.add('correct');
  });

  if (q.options[optIdx].correct) {
    el.classList.add('correct');
    score++;
    feedback.className = 'quiz-feedback show correct';
  } else {
    el.classList.add('incorrect');
    feedback.className = 'quiz-feedback show incorrect';
  }
  feedback.textContent = q.options[optIdx].explanation;
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
  else message = 'Necesitas repasar el contenido. Revisa cada parametro y vuelve a intentarlo.';

  container.innerHTML = `
    <div class="quiz-results">
      <div class="score">${score}/${quizData.length}</div>
      <div class="score-label">${pct}% de respuestas correctas</div>
      <p class="score-message">${message}</p>
      <button class="btn btn-primary" onclick="renderQuiz()">Reiniciar evaluacion</button>
      <button class="btn btn-outline" style="margin-left:.5rem" onclick="navigateTo('inicio')">Volver al inicio</button>
    </div>
  `;
}

// Expose functions globally
window.selectOption = selectOption;
window.nextQuestion = nextQuestion;
window.renderQuiz = renderQuiz;

// ===== Hash-based navigation =====
function handleHash() {
  const hash = window.location.hash.replace('#', '');
  if (hash) navigateTo(hash);
}
window.addEventListener('hashchange', handleHash);
if (window.location.hash) handleHash();
