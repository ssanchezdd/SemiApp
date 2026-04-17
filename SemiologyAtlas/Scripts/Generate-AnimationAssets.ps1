param(
    [string]$OutputDir = (Join-Path $PSScriptRoot "..\Resources\Images")
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

function New-Color([string]$hex) { [System.Drawing.ColorTranslator]::FromHtml($hex) }
function New-Brush([string]$hex) { [System.Drawing.SolidBrush]::new((New-Color $hex)) }

function Draw-Rect($g, [string]$fill, [float]$x, [float]$y, [float]$w, [float]$h, [string]$stroke = "#D8E0E3")
{
    $fillBrush = New-Brush $fill
    $pen = [System.Drawing.Pen]::new((New-Color $stroke), 2)
    $g.FillRectangle($fillBrush, $x, $y, $w, $h)
    $g.DrawRectangle($pen, $x, $y, $w, $h)
    $fillBrush.Dispose()
    $pen.Dispose()
}

function Draw-Text($g, [string]$text, $font, $brush, [float]$x, [float]$y, [float]$w, [float]$h, [string]$align = "Near")
{
    $rect = [System.Drawing.RectangleF]::new($x, $y, $w, $h)
    $fmt = [System.Drawing.StringFormat]::new()
    $fmt.Alignment = [System.Drawing.StringAlignment]::$align
    $fmt.LineAlignment = [System.Drawing.StringAlignment]::Near
    $g.DrawString($text, $font, $brush, $rect, $fmt)
    $fmt.Dispose()
}

function Draw-Bullets($g, [string[]]$items, $font, $brush, [string]$accent, [float]$x, [float]$y, [float]$w)
{
    $dot = New-Brush $accent
    $cursor = $y
    foreach ($item in $items)
    {
        $g.FillEllipse($dot, $x, $cursor + 8, 10, 10)
        Draw-Text $g $item $font $brush ($x + 18) $cursor ($w - 18) 54
        $cursor += 58
    }
    $dot.Dispose()
}

function Draw-Arrow($g, $pen, [float]$x1, [float]$y1, [float]$x2, [float]$y2)
{
    $g.DrawLine($pen, $x1, $y1, $x2, $y2)
    $angle = [Math]::Atan2($y2 - $y1, $x2 - $x1)
    $size = 10.0
    $g.DrawLine($pen, $x2, $y2, $x2 - $size * [Math]::Cos($angle - 0.45), $y2 - $size * [Math]::Sin($angle - 0.45))
    $g.DrawLine($pen, $x2, $y2, $x2 - $size * [Math]::Cos($angle + 0.45), $y2 - $size * [Math]::Sin($angle + 0.45))
}

function Draw-StickPerson($g, $pen, [float]$x, [float]$y)
{
    $g.DrawEllipse($pen, $x, $y, 42, 42)
    $g.DrawLine($pen, $x + 21, $y + 42, $x + 21, $y + 120)
    $g.DrawLine($pen, $x + 21, $y + 62, $x - 18, $y + 86)
    $g.DrawLine($pen, $x + 21, $y + 62, $x + 62, $y + 78)
    $g.DrawLine($pen, $x + 21, $y + 120, $x - 8, $y + 164)
    $g.DrawLine($pen, $x + 21, $y + 120, $x + 50, $y + 158)
}

function Draw-Footprints($g, $brush, [int]$frameIndex, [string]$mode)
{
    $points = if ($mode -eq "cerebellar_gait")
    {
        switch ($frameIndex)
        {
            0 { @(@(98, 298), @(150, 314), @(206, 290), @(238, 322)) }
            1 { @(@(98, 296), @(144, 318), @(192, 286), @(240, 326)) }
            default { @(@(92, 296), @(144, 320), @(198, 284), @(242, 328)) }
        }
    }
    else
    {
        switch ($frameIndex)
        {
            0 { @(@(112, 316), @(138, 308), @(164, 314), @(190, 306)) }
            1 { @(@(112, 316), @(138, 308), @(160, 316), @(178, 314), @(194, 312)) }
            default { @(@(112, 316), @(138, 308), @(160, 316), @(178, 314), @(194, 312), @(208, 310)) }
        }
    }

    foreach ($p in $points)
    {
        $g.FillEllipse($brush, [float]$p[0], [float]$p[1], 14, 22)
        $g.FillEllipse($brush, [float]$p[0] + 14, [float]$p[1] - 4, 14, 22)
    }
}

function Draw-Icon($g, [string]$mode, [int]$frameIndex, [string]$accent)
{
    $basePen = [System.Drawing.Pen]::new((New-Color "#607080"), 6)
    $accentPen = [System.Drawing.Pen]::new((New-Color $accent), 6)
    $accentBrush = New-Brush $accent

    switch ($mode)
    {
        "general_survey" {
            $g.DrawRectangle($basePen, 76, 152, 184, 214)
            $g.DrawRectangle($basePen, 92, 168, 40, 180)
            Draw-StickPerson $g $basePen 146 180
            if ($frameIndex -eq 0) { $g.DrawEllipse($accentPen, 140, 174, 54, 54) }
            if ($frameIndex -eq 1) { $g.DrawArc($accentPen, 132, 210, 72, 98, 18, 152) }
            if ($frameIndex -eq 2) {
                $g.FillRectangle($accentBrush, 210, 204, 52, 16)
                $g.FillRectangle($accentBrush, 210, 236, 42, 16)
                $g.FillRectangle($accentBrush, 210, 268, 58, 16)
            }
        }
        "neurologic_screen" {
            $g.DrawEllipse($basePen, 88, 170, 70, 70)
            for ($i = 0; $i -lt 4; $i++) { $g.DrawLine($basePen, 196 + ($i * 14), 176, 196 + ($i * 14), 206) }
            $g.DrawLine($basePen, 116, 300, 186, 300)
            $g.DrawEllipse($basePen, 196, 286, 18, 26)
            $g.DrawEllipse($basePen, 226, 286, 18, 26)
            if ($frameIndex -eq 0) { $g.DrawEllipse($accentPen, 80, 162, 86, 86) }
            if ($frameIndex -eq 1) { for ($i = 0; $i -lt 4; $i++) { $g.DrawLine($accentPen, 196 + ($i * 14), 170, 196 + ($i * 14), 214) } }
            if ($frameIndex -eq 2) { $g.DrawLine($accentPen, 116, 300, 186, 300); $g.FillEllipse($accentBrush, 248, 314, 10, 10) }
        }
        "cardio_clues" {
            $g.DrawLine($basePen, 140, 156, 140, 206)
            $g.DrawLine($basePen, 196, 156, 196, 206)
            $g.DrawBezier($basePen, 152, 226, 144, 194, 182, 194, 170, 226)
            $g.DrawBezier($basePen, 170, 226, 182, 194, 220, 194, 188, 240)
            $g.DrawArc($basePen, 96, 308, 36, 52, 160, 170)
            $g.DrawArc($basePen, 204, 308, 36, 52, 210, 170)
            if ($frameIndex -eq 0) { $g.DrawLine($accentPen, 140, 156, 140, 206); $g.DrawLine($accentPen, 196, 156, 196, 206) }
            if ($frameIndex -eq 1) { $g.DrawBezier($accentPen, 152, 226, 144, 194, 182, 194, 170, 226); $g.DrawBezier($accentPen, 170, 226, 182, 194, 220, 194, 188, 240) }
            if ($frameIndex -eq 2) { $g.DrawArc($accentPen, 92, 304, 44, 60, 160, 170); $g.DrawArc($accentPen, 200, 304, 44, 60, 210, 170) }
        }
        "resp_pattern" {
            $g.DrawArc($basePen, 118, 156, 100, 92, 18, 144)
            $g.DrawLine($basePen, 168, 204, 168, 334)
            $g.DrawArc($basePen, 110, 208, 58, 116, 300, 120)
            $g.DrawArc($basePen, 168, 208, 58, 116, 120, 120)
            if ($frameIndex -eq 0) { for ($i = 0; $i -lt 3; $i++) { $g.DrawArc($accentPen, 238 + ($i * 22), 206, 24, 26, 120, 180) } }
            if ($frameIndex -eq 1) { Draw-Arrow $g $accentPen 118 234 78 218; Draw-Arrow $g $accentPen 218 234 258 218 }
            if ($frameIndex -eq 2) { $g.DrawLine($accentPen, 112, 250, 224, 250); $g.DrawLine($accentPen, 112, 288, 224, 288) }
        }
        "gi_survey" {
            $g.DrawArc($basePen, 118, 156, 100, 92, 18, 144)
            $g.DrawLine($basePen, 136, 220, 124, 328)
            $g.DrawLine($basePen, 200, 220, 212, 328)
            $g.DrawArc($basePen, 118, 234, 100, 90, 180, 180)
            if ($frameIndex -eq 0) { $g.DrawArc($accentPen, 114, 230, 108, 98, 180, 180) }
            if ($frameIndex -eq 1) { $g.DrawLine($accentPen, 132, 270, 208, 270); Draw-Arrow $g $accentPen 88 290 124 290 }
            if ($frameIndex -eq 2) { $g.DrawLine($accentPen, 242, 178, 260, 214); $g.DrawLine($accentPen, 260, 178, 242, 214) }
        }
        "bradykinesia" {
            $levels = switch ($frameIndex) { 0 { @(44,44,44,44,44) } 1 { @(44,36,28,20,14) } default { @(46,32,22,14,8) } }
            for ($i = 0; $i -lt $levels.Count; $i++) {
                $x = 94 + ($i * 34)
                $g.DrawLine($basePen, $x, 320, $x, 220)
                $g.FillEllipse($accentBrush, $x - 8, 320 - $levels[$i], 16, 16)
            }
        }
        "rigidity" {
            $g.DrawEllipse($basePen, 106, 232, 28, 28)
            $g.DrawLine($basePen, 120, 246, 170, 214)
            $g.DrawLine($basePen, 170, 214, 228, 240)
            if ($frameIndex -eq 0) { Draw-Arrow $g $accentPen 88 264 138 264; Draw-Arrow $g $accentPen 258 204 212 204 }
            if ($frameIndex -eq 1) { for ($i = 0; $i -lt 4; $i++) { $g.FillRectangle($accentBrush, 138 + ($i * 22), 258, 12, 34) } }
            if ($frameIndex -eq 2) { for ($i = 0; $i -lt 3; $i++) { $g.DrawLine($accentPen, 126 + ($i * 32), 282, 136 + ($i * 32), 270); $g.DrawLine($accentPen, 136 + ($i * 32), 270, 146 + ($i * 32), 282) } }
        }
        "parkinson_tremor" {
            $g.DrawArc($basePen, 104, 184, 92, 80, 0, 180)
            $g.DrawLine($basePen, 86, 286, 236, 286)
            if ($frameIndex -eq 1) { Draw-Rect $g "#FFF7E8" 212 166 64 38 $accent }
            if ($frameIndex -eq 2) { Draw-Arrow $g $accentPen 164 208 244 176 }
            $waveCount = if ($frameIndex -eq 0) { 3 } elseif ($frameIndex -eq 1) { 4 } else { 1 }
            for ($i = 0; $i -lt $waveCount; $i++) { $g.DrawArc($accentPen, 226 + ($i * 14), 228, 20, 28, 80, 200) }
        }
        "essential_tremor" {
            if ($frameIndex -eq 0) {
                $g.DrawLine($basePen, 168, 188, 168, 270)
                $g.DrawLine($basePen, 168, 214, 112, 194)
                $g.DrawLine($basePen, 168, 214, 224, 194)
                for ($i = 0; $i -lt 3; $i++) { $g.DrawArc($accentPen, 88 + ($i * 18), 188, 18, 22, 90, 180); $g.DrawArc($accentPen, 228 + ($i * 18), 188, 18, 22, 90, 180) }
            }
            elseif ($frameIndex -eq 1) {
                $g.DrawEllipse($basePen, 118, 184, 100, 100)
                $g.DrawArc($accentPen, 122, 188, 92, 92, 20, 320)
            }
            else {
                Draw-Rect $g "#E9F3F8" 112 194 36 64 "#B7C9D3"
                $g.DrawLine($basePen, 148, 194, 196, 214)
                $g.DrawLine($basePen, 196, 214, 218, 264)
                for ($i = 0; $i -lt 3; $i++) { $g.DrawArc($accentPen, 204 + ($i * 12), 216, 18, 24, 90, 180) }
            }
        }
        "chorea" {
            Draw-StickPerson $g $basePen 146 172
            $delta = if ($frameIndex -eq 1) { 12 } elseif ($frameIndex -eq 2) { -10 } else { 0 }
            Draw-Arrow $g $accentPen (112 + $delta) 200 (90 + $delta) 172
            Draw-Arrow $g $accentPen (228 - $delta) 232 (256 - $delta) 214
            Draw-Arrow $g $accentPen 132 324 110 350
        }
        "dystonia" {
            Draw-StickPerson $g $basePen 152 170
            if ($frameIndex -eq 0) { $g.DrawArc($accentPen, 130, 160, 86, 86, 210, 170) }
            elseif ($frameIndex -eq 1) { Draw-Rect $g "#EEF3F7" 106 262 90 48 "#C5CFD6"; $g.DrawLine($accentPen, 154, 262, 212, 246) }
            else { $g.FillEllipse($accentBrush, 126, 178, 16, 16); Draw-Arrow $g $accentPen 122 186 146 188 }
        }
        "myoclonus" {
            Draw-StickPerson $g $basePen 146 170
            $bursts = if ($frameIndex -eq 0) { @(@(214,206), @(112,282)) } elseif ($frameIndex -eq 1) { @(@(226,182), @(108,286), @(86,188)) } else { @(@(226,180), @(108,280), @(212,326), @(120,206)) }
            foreach ($b in $bursts) {
                $x = [float]$b[0]; $y = [float]$b[1]
                $g.DrawLine($accentPen, $x - 8, $y, $x + 8, $y); $g.DrawLine($accentPen, $x, $y - 8, $x, $y + 8)
                $g.DrawLine($accentPen, $x - 6, $y - 6, $x + 6, $y + 6); $g.DrawLine($accentPen, $x - 6, $y + 6, $x + 6, $y - 6)
            }
            if ($frameIndex -eq 1) { Draw-Rect $g "#FFF8E8" 82 168 28 22 $accent }
        }
        "parkinson_gait" {
            $g.DrawLine($basePen, 86, 334, 252, 334)
            Draw-Rect $g "#F5F8F6" 226 192 30 90 "#D2D7D1"
            Draw-Footprints $g $accentBrush $frameIndex $mode
            if ($frameIndex -ge 1) { Draw-Arrow $g $accentPen 182 252 226 252 }
        }
        "cerebellar_gait" {
            Draw-Footprints $g $accentBrush $frameIndex $mode
            if ($frameIndex -ge 1) { $g.DrawLine($accentPen, 110, 182, 244, 182) }
            if ($frameIndex -eq 2) { Draw-Arrow $g $accentPen 90 240 126 226 }
        }
    }

    $basePen.Dispose()
    $accentPen.Dispose()
    $accentBrush.Dispose()
}

function New-Frame([string]$esCap, [string]$enCap, [string[]]$esBullets, [string[]]$enBullets, [string]$esCall, [string]$enCall)
{
    [pscustomobject]@{
        CaptionEs = $esCap
        CaptionEn = $enCap
        BulletsEs = $esBullets
        BulletsEn = $enBullets
        CalloutEs = $esCall
        CalloutEn = $enCall
    }
}

function New-Topic([string]$key, [string]$accent, [string]$tagEs, [string]$tagEn, [string]$titleEs, [string]$titleEn, [object[]]$frames)
{
    [pscustomobject]@{
        Key = $key
        Accent = $accent
        TagEs = $tagEs
        TagEn = $tagEn
        TitleEs = $titleEs
        TitleEn = $titleEn
        Frames = $frames
    }
}

$topics = @(
    (New-Topic "general_survey" "#0F766E" "Aspecto general" "General appearance" "Encuesta general" "General survey" @(
        (New-Frame "Puerta" "Doorway" @("Describe alerta, distres y edad aparente.", "Mira postura, movilidad y vestido.", "La encuesta general ocurre antes de tocar.") @("Describe alertness, distress, and apparent age.", "Watch posture, mobility, and dress.", "The general survey happens before touching.") "Primera impresion en 30 segundos." "First impression in 30 seconds."),
        (New-Frame "Perfusion" "Perfusion" @("Palidez, cianosis e ictericia cambian gravedad.", "Mucosas secas y caquexia sugieren enfermedad sistemica.", "Color e hidratacion orientan estabilidad.") @("Pallor, cyanosis, and jaundice change severity.", "Dry mucosa and cachexia suggest systemic disease.", "Color and hydration orient stability.") "Color y contexto antes del examen focal." "Color and context before the focused exam."),
        (New-Frame "Resumen" "Summary" @("Cierra con una frase clinica de una linea.", "Separa encuesta general de examen focal.", "Puerta, postura, perfusion y presentacion.") @("Close with a one-line clinical summary.", "Keep the survey separate from the focused exam.", "Doorway, posture, perfusion, and presentation.") "Resume antes de profundizar." "Summarize before going deeper.")
    )),
    (New-Topic "neurologic_screen" "#1D4ED8" "Sistema nervioso" "Neurological system" "Tamizaje neurologico" "Neurological screen" @(
        (New-Frame "Cara y voz" "Face and voice" @("Busca hipomimia, parpadeo pobre y voz baja.", "La gesticulacion espontanea cae en hipocinesia.", "Empieza mirando antes de explorar manos.") @("Look for hypomimia, low blink rate, and soft voice.", "Spontaneous gesturing falls in hypokinesia.", "Start by watching before testing the hands.") "La cara ya da pistas de parkinsonismo." "The face already hints at parkinsonism."),
        (New-Frame "Manos y tono" "Hands and tone" @("Haz finger tapping y abrir-cerrar manos.", "Palpa tono pasivo para separar rigidez de espasticidad.", "La secuencia importa mas que una repeticion aislada.") @("Use finger tapping and hand opening-closing.", "Check passive tone to separate rigidity from spasticity.", "Sequence matters more than one isolated repetition.") "Velocidad, amplitud y tono." "Speed, amplitude, and tone."),
        (New-Frame "Marcha y patron" "Gait and pattern" @("Describe base, paso, braceo y giros.", "Decide si domina hipocinesia, hipercinesia o deficit focal.", "Nombrar el sindrome viene despues de describir.") @("Describe base, stride, arm swing, and turns.", "Decide whether hypokinesia, hyperkinesia, or focal deficit dominates.", "Naming the syndrome comes after describing it.") "Cara, manos, tono y marcha." "Face, hands, tone, and gait.")
    )),
    (New-Topic "cardio_clues" "#B91C1C" "Cardiovascular" "Cardiovascular" "Claves cardiovasculares" "Cardiovascular clues" @(
        (New-Frame "Cuello" "Neck veins" @("Mira yugulares a 45 grados.", "La altura venosa ayuda a reconocer congestion.", "No empieces por el estetoscopio.") @("Inspect neck veins at 45 degrees.", "Venous height helps recognize congestion.", "Do not start with the stethoscope.") "El cuello resume la presion venosa." "The neck summarizes venous pressure."),
        (New-Frame "Corazon y pulmon" "Heart and lungs" @("Ausculta soplos, S3 y crepitos bibasales.", "La irradiacion vale mas que el volumen aislado.", "Congestion pulmonar y cardiaca suelen coexistir.") @("Listen for murmurs, S3, and bibasal crackles.", "Radiation matters more than loudness alone.", "Pulmonary and cardiac congestion often coexist.") "Triada de congestion en contexto." "Congestion triad in context."),
        (New-Frame "Perfusion y edema" "Perfusion and edema" @("Pulso debil y piel fria sugieren bajo gasto.", "Edema bilateral apoya sobrecarga de volumen.", "Une cuello, pulmon y piernas en un sindrome.") @("Weak pulse and cool skin suggest low output.", "Bilateral edema supports volume overload.", "Link neck, lungs, and legs into one syndrome.") "Piensa en sindrome antes que en eco." "Think syndrome before echo.")
    )),
    (New-Topic "resp_pattern" "#2563EB" "Respiratorio" "Respiratory" "Patron respiratorio" "Respiratory pattern" @(
        (New-Frame "Frecuencia y frase" "Rate and speech" @("Cuenta respiraciones por minuto.", "Si no habla frases completas, ya es grave.", "La frecuencia respiratoria no se estima a ojo.") @("Count respirations per minute.", "If the patient cannot speak full sentences, severity is high.", "Respiratory rate should not be guessed.") "Primero define estabilidad ventilatoria." "First define ventilatory stability."),
        (New-Frame "Trabajo ventilatorio" "Work of breathing" @("Busca tiraje, aleteo nasal y tripode.", "Musculos accesorios indican carga alta.", "Obstruccion y fatiga pueden coexistir.") @("Look for retractions, nasal flaring, and tripod posture.", "Accessory muscles indicate high work.", "Obstruction and fatigue can coexist.") "El esfuerzo importa tanto como el ruido." "Effort matters as much as sound."),
        (New-Frame "Torax silencioso" "Silent chest" @("Silencio auscultatorio en un paciente agotado es bandera roja.", "No confundas menos ruido con mejoria.", "Actua rapido si hay somnolencia o cianosis.") @("A silent chest in a tiring patient is a red flag.", "Do not mistake less sound for improvement.", "Act quickly if drowsiness or cyanosis appear.") "Torax silencioso puede ser peor que sibilancias." "A silent chest can be worse than wheeze.")
    )),
    (New-Topic "gi_survey" "#B45309" "Abdomen e higado" "Abdomen and liver" "Abdomen y pistas sistemicas" "Abdomen and systemic clues" @(
        (New-Frame "Inspeccion" "Inspection" @("Mira distension, cicatrices, venas y masa muscular.", "La piel puede sugerir hepatopatia avanzada.", "No todo es palpacion del dolor.") @("Inspect distension, scars, veins, and muscle mass.", "The skin can suggest advanced liver disease.", "It is not all about pain on palpation.") "La inspeccion ya organiza el patron." "Inspection already organizes the pattern."),
        (New-Frame "Percusion" "Percussion" @("Busca ascitis, timpanismo y dolor localizado.", "La matidez cambiante distingue liquido.", "Relaciona defensa con necesidad de cirugia.") @("Look for ascites, tympany, and localized pain.", "Shifting dullness helps identify fluid.", "Relate guarding to surgical urgency.") "Liquido, aire o irritacion peritoneal." "Fluid, air, or peritoneal irritation."),
        (New-Frame "Higado y sistema" "Liver and system" @("Asterixis y confusion sugieren encefalopatia.", "Ictericia, ascitis y equimosis indican descompensacion.", "Une abdomen con estado mental y piel.") @("Asterixis and confusion suggest encephalopathy.", "Jaundice, ascites, and bruising suggest decompensation.", "Link abdomen with skin and mental status.") "El higado se ve en piel, abdomen y cerebro." "The liver shows in skin, abdomen, and brain.")
    )),
    (New-Topic "bradykinesia" "#4D7C0F" "Hipocinetico" "Hypokinetic" "Bradicinesia" "Bradykinesia" @(
        (New-Frame "Inicio" "Start" @("Pide finger tapping rapido y repetido.", "El signo no es solo lentitud.", "Necesitas una secuencia, no un gesto unico.") @("Ask for rapid repeated finger tapping.", "The sign is not just slowness.", "You need a sequence, not a single gesture.") "Primero provoca repeticion." "First provoke repetition."),
        (New-Frame "Decremento" "Decrement" @("La amplitud se hace mas pequena.", "La velocidad cae conforme avanzan las repeticiones.", "Ese agotamiento define el fenomeno.") @("Amplitude becomes smaller.", "Speed falls as repetitions continue.", "That sequence effect defines the phenomenon.") "La clave es el decremento." "The key is decrement."),
        (New-Frame "Interpretacion" "Interpretation" @("Bradicinesia es signo cardinal del parkinsonismo.", "Comparala con rigidez, temblor y braceo.", "No la confundas con debilidad o apatia.") @("Bradykinesia is a cardinal sign of parkinsonism.", "Compare it with rigidity, tremor, and arm swing.", "Do not confuse it with weakness or apathy.") "Lentitud mas perdida de amplitud." "Slowness plus loss of amplitude.")
    )),
    (New-Topic "rigidity" "#65A30D" "Hipocinetico" "Hypokinetic" "Rigidez" "Rigidity" @(
        (New-Frame "Movilizacion pasiva" "Passive movement" @("Relaja al paciente y mueve muneca o codo.", "La exploracion es pasiva, no voluntaria.", "Si el paciente ayuda, el signo se distorsiona.") @("Relax the patient and move the wrist or elbow.", "The test is passive, not voluntary.", "If the patient helps, the sign becomes distorted.") "Explora tono pasivo." "Assess passive tone."),
        (New-Frame "Resistencia uniforme" "Uniform resistance" @("La resistencia es similar en todo el trayecto.", "No depende de la velocidad.", "Eso la separa de la espasticidad.") @("Resistance is similar throughout the range.", "It does not depend on velocity.", "That separates it from spasticity.") "Misma resistencia todo el trayecto." "Same resistance all the way."),
        (New-Frame "Activacion" "Activation" @("Si es sutil, activa el lado contrario.", "Puede sentirse rueda dentada si se suma temblor.", "Relaciona rigidez con bradicinesia antes de llamar parkinsonismo.") @("If subtle, activate the opposite side.", "Cogwheeling may appear if tremor is superimposed.", "Relate rigidity to bradykinesia before calling it parkinsonism.") "Rigidez puede requerir maniobra activadora." "Rigidity may need an activation maneuver.")
    )),
    (New-Topic "parkinson_tremor" "#166534" "Hipocinetico" "Hypokinetic" "Temblor parkinsoniano" "Parkinsonian tremor" @(
        (New-Frame "Reposo" "Rest" @("Observa la mano apoyada y relajada.", "Suele ser asimetrico y de contar monedas.", "El estado es mas importante que el nombre.") @("Observe the supported, relaxed hand.", "It is often asymmetric and pill-rolling.", "State matters more than the nickname.") "Si domina en reposo, piensa en parkinsonismo." "If it dominates at rest, think parkinsonism."),
        (New-Frame "Distraccion" "Distraction" @("Cuenta hacia atras si el temblor es intermitente.", "La distraccion puede hacerlo mas visible.", "No olvides observar varias posiciones.") @("Count backward if tremor is intermittent.", "Distraction can make it more visible.", "Do not forget to observe several positions.") "La carga cognitiva puede revelar el temblor." "Cognitive load can reveal the tremor."),
        (New-Frame "Accion" "Action" @("Al alcanzar un objeto suele reducirse.", "Puede reaparecer al sostener postura.", "Buscalo junto a hipomimia y bradicinesia.") @("It often lessens while reaching for an object.", "It may re-emerge while holding posture.", "Look for hypomimia and bradykinesia with it.") "Describe reposo, postura y accion." "Describe rest, posture, and action.")
    )),
    (New-Topic "essential_tremor" "#F97316" "Hipercinetico" "Hyperkinetic" "Temblor esencial" "Essential tremor" @(
        (New-Frame "Postura" "Posture" @("Se acentua con brazos extendidos.", "Suele ser bilateral.", "Cabeza y voz tambien pueden temblar.") @("It increases with outstretched arms.", "It is usually bilateral.", "Head and voice may also be involved.") "Es un temblor de accion." "It is an action tremor."),
        (New-Frame "Tarea" "Task" @("Mira espiral, vaso o cuchara.", "La accion fina desenmascara el patron.", "Pregunta por historia familiar y alivio con alcohol.") @("Watch a spiral, a cup, or utensils.", "Fine action unmasks the pattern.", "Ask about family history and alcohol response.") "El objetivo es provocar accion sostenida." "The goal is to provoke sustained action."),
        (New-Frame "Diferencia" "Difference" @("No debe haber bradicinesia ni rigidez verdadera.", "Si el temblor domina en reposo, revisa el diagnostico.", "Contrasta accion contra reposo.") @("There should be no true bradykinesia or rigidity.", "If tremor dominates at rest, revisit the diagnosis.", "Contrast action against rest.") "Postural o cinetico, no de reposo predominante." "Postural or kinetic, not predominantly rest.")
    )),
    (New-Topic "chorea" "#F59E0B" "Hipercinetico" "Hyperkinetic" "Corea" "Chorea" @(
        (New-Frame "Irregular" "Irregular" @("Los movimientos migran y no son ritmicos.", "Parecen inquietud pero cambian de region.", "La espontaneidad los muestra mejor.") @("The movements migrate and are not rhythmic.", "They may look like restlessness but change regions.", "Spontaneous behavior shows them best.") "No hay oscilacion regular." "There is no regular oscillation."),
        (New-Frame "Impersistencia" "Impersistence" @("Pide protrusion lingual o prension sostenida.", "La postura se pierde rapido.", "Busca prension lechera y lengua danzante.") @("Ask for sustained tongue protrusion or grip.", "The posture is quickly lost.", "Look for milkmaid grip and darting tongue.") "La postura no se sostiene." "The posture does not hold."),
        (New-Frame "Camuflaje" "Camouflage" @("Puede esconderse en gestos semivoluntarios.", "No la llames temblor si no oscila.", "Describe fluyente, aleatoria y no ritmica.") @("It can hide inside semipurposeful gestures.", "Do not call it tremor if there is no oscillation.", "Describe it as flowing, random, and non-rhythmic.") "Corea es desborde impredecible." "Chorea is unpredictable overflow.")
    )),
    (New-Topic "dystonia" "#EA580C" "Hipercinetico" "Hyperkinetic" "Distonia" "Dystonia" @(
        (New-Frame "Postura anormal" "Abnormal posture" @("Hay torsion o posicion repetida.", "Puede ser sostenida o intermitente.", "La direccion del movimiento importa.") @("There is twisting or a repeated posture.", "It may be sustained or intermittent.", "The direction of movement matters.") "La postura se impone una y otra vez." "The posture keeps imposing itself."),
        (New-Frame "Especifica de tarea" "Task specific" @("Escritura, cuello o marcha pueden gatillarla.", "Provocala con la tarea correcta.", "No esperes verla solo en reposo.") @("Writing, neck use, or gait may trigger it.", "Provoke it with the right task.", "Do not expect it only at rest.") "La tarea correcta destapa la distonia." "The right task exposes dystonia."),
        (New-Frame "Truco sensorial" "Sensory trick" @("Un toque ligero puede aliviarla.", "Ese dato apoya distonia cervical.", "No la confundas con espasticidad fija.") @("A light touch may relieve it.", "That clue supports cervical dystonia.", "Do not confuse it with fixed spasticity.") "Busca alivio con gesto sensorial." "Look for relief with a sensory gesture.")
    )),
    (New-Topic "myoclonus" "#FB923C" "Hipercinetico" "Hyperkinetic" "Mioclonus" "Myoclonus" @(
        (New-Frame "Sacudida" "Jerk" @("Es una descarga muy breve, tipo shock.", "No es oscilatorio como el temblor.", "Puede ser focal o multifocal.") @("It is a very brief, shock-like discharge.", "It is not oscillatory like tremor.", "It may be focal or multifocal.") "Piensa en descarga, no en flujo." "Think discharge, not flow."),
        (New-Frame "Estimulo" "Stimulus" @("Sonido, tacto o movimiento pueden desencadenarlo.", "Explora sensibilidad a estimulos.", "Describe si aparece en reposo o accion.") @("Sound, touch, or movement may trigger it.", "Test stimulus sensitivity.", "Describe whether it appears at rest or with action.") "Los estimulos pueden precipitarlo." "Stimuli may precipitate it."),
        (New-Frame "Contexto" "Context" @("No olvides causas metabolicas, toxicas o epilepticas.", "Relaciona el signo con el estado general.", "La semiologia manda el siguiente paso.") @("Do not forget metabolic, toxic, or epileptic causes.", "Link the sign to the general clinical state.", "Semiology guides the next step.") "Mioclonus suele ser pista sistemica." "Myoclonus is often a systemic clue.")
    )),
    (New-Topic "parkinson_gait" "#374151" "Marcha" "Gait" "Marcha parkinsoniana" "Parkinsonian gait" @(
        (New-Frame "Paso corto" "Short stride" @("Postura flexionada y pasos pequenos.", "El braceo cae, a veces de forma asimetrica.", "No la describas solo como marcha lenta.") @("Stooped posture and small steps.", "Arm swing falls, sometimes asymmetrically.", "Do not describe it only as slow gait.") "Base, paso y braceo." "Base, stride, and arm swing."),
        (New-Frame "Giro" "Turning" @("El giro es en bloque y puede haber congelamiento.", "Puertas y cambios de direccion revelan el signo.", "Cuenta pasos para girar 180 grados.") @("Turning is en bloc and freezing may appear.", "Doorways and direction changes reveal the sign.", "Count steps needed for a 180-degree turn.") "Los giros muestran mas que la recta." "Turns reveal more than the straight walk."),
        (New-Frame "Doble tarea" "Dual task" @("Contar o hablar puede empeorar la marcha.", "Busca pasos correctivos y retropulsion.", "Relaciona marcha con bradicinesia y rigidez.") @("Counting or talking may worsen gait.", "Look for corrective steps and retropulsion.", "Relate gait to bradykinesia and rigidity.") "Transiciones y giros son clave." "Transitions and turns are key.")
    )),
    (New-Topic "cerebellar_gait" "#14B8A6" "Marcha" "Gait" "Marcha cerebelosa" "Cerebellar gait" @(
        (New-Frame "Base amplia" "Wide base" @("La marcha es ancha e inestable.", "La oscilacion lateral refleja mala coordinacion.", "No es una marcha hipocinetica.") @("The gait is broad and unstable.", "Lateral veering reflects poor coordination.", "It is not a hypokinetic gait.") "La base se abre para compensar." "The base widens to compensate."),
        (New-Frame "Tandem" "Tandem" @("La marcha en tandem suele fracasar.", "Los giros son erraticos, no en bloque.", "La ataxia descompone el movimiento.") @("Tandem gait often fails.", "Turns are erratic, not en bloc.", "Ataxia decomposes movement.") "Tandem desenmascara la ataxia." "Tandem exposes ataxia."),
        (New-Frame "Acompanantes" "Associates" @("Busca dismetria, nistagmo y habla escandida.", "Une miembros, ojos y marcha.", "Contrasta con paso corto parkinsoniano.") @("Look for dysmetria, nystagmus, and scanning speech.", "Link limbs, eyes, and gait.", "Contrast it with the short-step parkinsonian gait.") "Coordinacion y balance, no pobreza de movimiento." "Coordination and balance, not paucity of movement.")
    ))
)

if (-not (Test-Path $OutputDir))
{
    New-Item -ItemType Directory -Path $OutputDir | Out-Null
}

$titleFont = [System.Drawing.Font]::new("Segoe UI", 28, [System.Drawing.FontStyle]::Bold)
$tagFont = [System.Drawing.Font]::new("Segoe UI", 13, [System.Drawing.FontStyle]::Bold)
$captionFont = [System.Drawing.Font]::new("Segoe UI", 22, [System.Drawing.FontStyle]::Bold)
$bodyFont = [System.Drawing.Font]::new("Segoe UI", 14, [System.Drawing.FontStyle]::Regular)
$calloutFont = [System.Drawing.Font]::new("Segoe UI", 13, [System.Drawing.FontStyle]::Bold)
$stepFont = [System.Drawing.Font]::new("Segoe UI", 11, [System.Drawing.FontStyle]::Bold)
$footerFont = [System.Drawing.Font]::new("Segoe UI", 11, [System.Drawing.FontStyle]::Regular)
$whiteBrush = New-Brush "#FFFFFF"
$inkBrush = New-Brush "#22303A"
$mutedBrush = New-Brush "#586572"

foreach ($topic in $topics)
{
    foreach ($lang in @("es", "en"))
    {
        for ($frameIndex = 0; $frameIndex -lt $topic.Frames.Count; $frameIndex++)
        {
            $frame = $topic.Frames[$frameIndex]
            $tagText = if ($lang -eq "es") { $topic.TagEs } else { $topic.TagEn }
            $titleText = if ($lang -eq "es") { $topic.TitleEs } else { $topic.TitleEn }
            $captionText = if ($lang -eq "es") { $frame.CaptionEs } else { $frame.CaptionEn }
            $bulletItems = if ($lang -eq "es") { $frame.BulletsEs } else { $frame.BulletsEn }
            $calloutText = if ($lang -eq "es") { $frame.CalloutEs } else { $frame.CalloutEn }
            $bitmap = [System.Drawing.Bitmap]::new(800, 520)
            $g = [System.Drawing.Graphics]::FromImage($bitmap)
            $g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
            $g.TextRenderingHint = [System.Drawing.Text.TextRenderingHint]::ClearTypeGridFit
            $g.Clear((New-Color "#F5EFE7"))

            $header = [System.Drawing.Drawing2D.LinearGradientBrush]::new(
                [System.Drawing.Rectangle]::new(0, 0, 800, 118),
                (New-Color $topic.Accent),
                (New-Color "#132531"),
                0.0)
            $g.FillRectangle($header, 0, 0, 800, 118)

            Draw-Rect $g "#FFFDFC" 36 140 292 322
            Draw-Rect $g "#FFFDFC" 352 140 412 322

            Draw-Text $g $tagText $tagFont $whiteBrush 40 22 220 20
            Draw-Text $g $titleText $titleFont $whiteBrush 40 46 540 40
            Draw-Rect $g $topic.Accent 648 30 112 30 $topic.Accent
            Draw-Text $g ($(if ($lang -eq "es") { "Paso $($frameIndex + 1)" } else { "Step $($frameIndex + 1)" })) $stepFont $whiteBrush 648 37 112 20 "Center"

            Draw-Icon $g $topic.Key $frameIndex $topic.Accent
            Draw-Text $g $captionText $calloutFont $mutedBrush 58 398 248 22 "Center"

            Draw-Text $g $captionText $captionFont $inkBrush 376 168 340 32
            Draw-Bullets $g $bulletItems $bodyFont $inkBrush $topic.Accent 376 220 340

            Draw-Rect $g "#F3F7F8" 376 382 344 54 "#D4DFE3"
            Draw-Text $g $calloutText $calloutFont $inkBrush 392 398 312 26

            Draw-Text $g "Semiology Atlas" $footerFont $mutedBrush 40 484 180 18
            Draw-Text $g ($(if ($lang -eq "es") { "Animacion PNG educativa" } else { "Educational PNG animation" })) $footerFont $mutedBrush 536 484 220 18 "Center"

            $path = Join-Path $OutputDir ("{0}_{1}_{2}.png" -f $topic.Key, $lang, ($frameIndex + 1).ToString("00"))
            $bitmap.Save($path, [System.Drawing.Imaging.ImageFormat]::Png)

            $header.Dispose()
            $g.Dispose()
            $bitmap.Dispose()
        }
    }
}

$titleFont.Dispose()
$tagFont.Dispose()
$captionFont.Dispose()
$bodyFont.Dispose()
$calloutFont.Dispose()
$stepFont.Dispose()
$footerFont.Dispose()
$whiteBrush.Dispose()
$inkBrush.Dispose()
$mutedBrush.Dispose()

Write-Host ("Generated {0} PNG files in {1}" -f ($topics.Count * 6), (Resolve-Path $OutputDir))
