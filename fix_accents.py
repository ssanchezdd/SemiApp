"""Apply Spanish accent fixes to docs/index.html and docs/js/app.js.

The Flash-sourced content was typed without accents. This script runs
conservative, word-boundary replacements that are safe for medical Spanish:

1. Protect HTML attribute values (id, href, data-section, class, src, for) and
   JS `navigateTo('X')` calls so anchor IDs stay intact (e.g. id="orientacion").
2. Any word ending in -cion / -sion takes -ción / -sión (rule-based).
3. A curated list of specific words (nouns, adjectives) that categorically
   need an accent (e.g. clinica -> clínica).
4. Unprotect.
"""
import re

FILES = [
    r"d:\SemiApp\docs\index.html",
    r"d:\SemiApp\docs\js\app.js",
]

# Curated word list. Word boundaries are enforced; all keys are ASCII.
WORDS = {
    # Anatomy / nouns
    "torax": "tórax",
    "craneo": "cráneo",
    "diametro": "diámetro", "diametros": "diámetros",
    "estomago": "estómago",
    "higado": "hígado",
    "musculo": "músculo", "musculos": "músculos",
    "organo": "órgano", "organos": "órganos",
    "apendice": "apéndice", "apendices": "apéndices",
    "parpado": "párpado", "parpados": "párpados",
    "pomulo": "pómulo", "pomulos": "pómulos",
    "gluteo": "glúteo", "gluteos": "glúteos",
    "visceras": "vísceras", "viscera": "víscera",
    "celula": "célula", "celulas": "células",
    "oxigeno": "oxígeno",
    "liquido": "líquido", "liquidos": "líquidos",
    "liquida": "líquida", "liquidas": "líquidas",
    "via": "vía", "vias": "vías",
    "lobulo": "lóbulo", "lobulos": "lóbulos",
    "cateter": "catéter", "cateteres": "catéteres",
    "Cateteres": "Catéteres",
    "sindrome": "síndrome", "Sindrome": "Síndrome",
    "sindromes": "síndromes",
    "paralisis": "parálisis",
    "analisis": "análisis",
    "diagnostico": "diagnóstico", "diagnosticos": "diagnósticos",
    "Diagnostico": "Diagnóstico",
    "sintoma": "síntoma", "sintomas": "síntomas",
    "hipotesis": "hipótesis",
    "estimulo": "estímulo", "estimulos": "estímulos",
    "angulo": "ángulo", "angulos": "ángulos",
    "numero": "número", "numeros": "números",
    "metodo": "método", "metodos": "métodos",
    "habito": "hábito", "habitos": "hábitos",
    "parametro": "parámetro", "parametros": "parámetros",
    "Parametro": "Parámetro", "Parametros": "Parámetros",
    "caracter": "carácter",
    "caracteristico": "característico", "caracteristicos": "característicos",
    "caracteristica": "característica", "caracteristicas": "características",
    "indole": "índole",
    "imagenes": "imágenes", "Imagenes": "Imágenes",
    "examenes": "exámenes",
    "sincope": "síncope",
    "animo": "ánimo", "Animo": "Ánimo",

    # Fields / disciplines
    "Semiologia": "Semiología", "semiologia": "semiología",
    "tecnologia": "tecnología", "tecnologias": "tecnologías",
    "tomografia": "tomografía", "ecografia": "ecografía",
    "radiografia": "radiografía",
    "bibliografia": "bibliografía", "Bibliografia": "Bibliografía",
    "patologia": "patología", "patologias": "patologías",
    "categoria": "categoría", "categorias": "categorías",
    "subcategoria": "subcategoría", "subcategorias": "subcategorías",
    "asimetria": "asimetría",
    "mioclonia": "mioclonía", "mioclonias": "mioclonías",
    "teoria": "teoría", "teorias": "teorías",
    "propedeutica": "propedéutica",
    "traqueotomia": "traqueotomía",

    # Adjectives
    "clinico": "clínico", "clinica": "clínica",
    "clinicos": "clínicos", "clinicas": "clínicas",
    "Clinico": "Clínico", "Clinica": "Clínica",
    "medico": "médico", "medica": "médica",
    "medicos": "médicos", "medicas": "médicas",
    "practica": "práctica", "practicas": "prácticas",
    "practico": "práctico", "practicos": "prácticos",
    "fisico": "físico", "fisica": "física",
    "fisicos": "físicos", "fisicas": "físicas",
    "tecnica": "técnica", "tecnicas": "técnicas",
    "tecnico": "técnico", "tecnicos": "técnicos",
    "rapido": "rápido", "rapidos": "rápidos",
    "rapida": "rápida", "rapidas": "rápidas",
    "ritmico": "rítmico", "ritmicos": "rítmicos",
    "ritmica": "rítmica", "ritmicas": "rítmicas",
    "arritmico": "arrítmico", "arritmicos": "arrítmicos",
    "arritmica": "arrítmica", "arritmicas": "arrítmicas",
    "tipico": "típico", "tipicos": "típicos",
    "tipica": "típica", "tipicas": "típicas",
    "clasico": "clásico", "clasicos": "clásicos",
    "clasica": "clásica", "clasicas": "clásicas",
    "ultimo": "último", "ultima": "última",
    "ultimos": "últimos", "ultimas": "últimas",
    "unico": "único", "unica": "única",
    "unicos": "únicos", "unicas": "únicas",
    "util": "útil", "utiles": "útiles",
    "facil": "fácil", "faciles": "fáciles",
    "dificil": "difícil", "dificiles": "difíciles",
    "debil": "débil", "debiles": "débiles",
    "inmovil": "inmóvil", "inmoviles": "inmóviles",
    "subito": "súbito", "subitos": "súbitos",
    "subita": "súbita", "subitas": "súbitas",
    "agil": "ágil", "agiles": "ágiles",
    "magnetica": "magnética", "magneticas": "magnéticas",
    "magnetico": "magnético", "magneticos": "magnéticos",
    "epigastrico": "epigástrico", "epigastrica": "epigástrica",
    "caquectico": "caquéctico", "caquectica": "caquéctica",
    "cronico": "crónico", "cronica": "crónica",
    "cronicos": "crónicos", "cronicas": "crónicas",
    "cronologico": "cronológico", "cronologica": "cronológica",
    "toxico": "tóxico", "toxica": "tóxica",
    "toxicos": "tóxicos", "toxicas": "tóxicas",
    "anatomico": "anatómico", "anatomica": "anatómica",
    "anatomicos": "anatómicos", "anatomicas": "anatómicas",
    "asimetrico": "asimétrico", "asimetrica": "asimétrica",
    "asimetricos": "asimétricos", "asimetricas": "asimétricas",
    "aerea": "aérea", "aereo": "aéreo",
    "aereas": "aéreas", "aereos": "aéreos",
    "miastenico": "miasténico", "miastenica": "miasténica",
    "rigido": "rígido", "rigida": "rígida",
    "rigidos": "rígidos", "rigidas": "rígidas",
    "paroxistico": "paroxístico", "paroxisticos": "paroxísticos",
    "paroxistica": "paroxística", "paroxisticas": "paroxísticas",
    "neurologico": "neurológico", "neurologica": "neurológica",
    "neurologicos": "neurológicos", "neurologicas": "neurológicas",
    "endogena": "endógena", "endogeno": "endógeno",
    "exogena": "exógena", "exogeno": "exógeno",
    "pletorica": "pletórica", "pletorico": "pletórico",
    "cianotica": "cianótica", "cianotico": "cianótico",
    "hepatico": "hepático", "hepatica": "hepática",
    "hepaticos": "hepáticos", "hepaticas": "hepáticas",
    "cetoacidotico": "cetoacidótico", "cetoacidotica": "cetoacidótica",
    "uremico": "urémico", "uremica": "urémica",
    "metabolico": "metabólico", "metabolicos": "metabólicos",
    "metabolica": "metabólica", "metabolicas": "metabólicas",
    "fisiologico": "fisiológico", "fisiologicos": "fisiológicos",
    "fisiologica": "fisiológica", "fisiologicas": "fisiológicas",
    "fisiopatologico": "fisiopatológico", "fisiopatologicos": "fisiopatológicos",
    "terapeutico": "terapéutico", "terapeuticos": "terapéuticos",
    "terapeutica": "terapéutica", "terapeuticas": "terapéuticas",
    "organico": "orgánico", "organica": "orgánica",
    "organicos": "orgánicos", "organicas": "orgánicas",
    "patologico": "patológico", "patologica": "patológica",
    "patologicos": "patológicos", "patologicas": "patológicas",
    "periferica": "periférica", "perifericas": "periféricas",
    "periferico": "periférico", "perifericos": "periféricos",
    "tonico": "tónico", "tonica": "tónica",
    "tonicos": "tónicos", "tonicas": "tónicas",
    "apraxico": "apráxico", "apraxica": "apráxica",
    "hemiplejico": "hemipléjico", "hemiplejica": "hemipléjica",
    "ataxico": "atáxico", "ataxica": "atáxica",
    "miopatica": "miopática", "miopatico": "miopático",
    "traumatico": "traumático", "traumatica": "traumática",
    "traumaticos": "traumáticos", "traumaticas": "traumáticas",
    "automatico": "automático", "automatica": "automática",
    "automaticos": "automáticos", "automaticas": "automáticas",
    "sanguineo": "sanguíneo", "sanguinea": "sanguínea",
    "sanguineos": "sanguíneos", "sanguineas": "sanguíneas",
    "laringeo": "laríngeo", "laringea": "laríngea",
    "laringeos": "laríngeos", "laringeas": "laríngeas",
    "carotideo": "carotídeo", "carotidea": "carotídea",
    "cardiaco": "cardíaco", "cardiacos": "cardíacos",
    "toracico": "torácico", "toracica": "torácica",
    "toracicos": "torácicos", "toracicas": "torácicas",
    "cientifico": "científico", "cientifica": "científica",
    "cientificos": "científicos", "cientificas": "científicas",
    "publico": "público", "publica": "pública",
    "publicos": "públicos", "publicas": "públicas",
    "academico": "académico", "academica": "académica",
    "academicos": "académicos", "academicas": "académicas",
    "quimico": "químico", "quimica": "química",
    "quimicos": "químicos", "quimicas": "químicas",
    "electrolito": "electrólito", "electrolitos": "electrólitos",
    "propedeutico": "propedéutico",
    "dolicocefalica": "dolicocefálica", "dolicocefalico": "dolicocefálico",
    "braquicefalica": "braquicefálica", "braquicefalico": "braquicefálico",
    "centripeta": "centrípeta",

    # Postures
    "opistotonos": "opistótonos", "Opistotonos": "Opistótonos",
    "pleurotonos": "pleurótonos", "Pleurotonos": "Pleurótonos",
    "emprostotonos": "emprostótonos", "Emprostotonos": "Emprostótonos",

    # Common adverbs / connectors
    "tambien": "también", "Tambien": "También",
    "ademas": "además", "Ademas": "Además",
    "despues": "después", "Despues": "Después",
    "asi": "así", "Asi": "Así",
    "aqui": "aquí", "Aqui": "Aquí",
    "alli": "allí", "Alli": "Allí",
    "alla": "allá", "Alla": "Allá",
    "mas": "más", "Mas": "Más",
    "dia": "día", "dias": "días",
    "pais": "país", "paises": "países",
    "estres": "estrés",
    "atras": "atrás",
    "adios": "adiós",
    "ahi": "ahí",

    # Proper nouns
    "Paris": "París",

    # Region / visión (end in -ion but need accent)
    "region": "región",
    "vision": "visión",

    # Medical-specific
    "Genero": "Género", "genero": "género",
    "acne": "acné",
    "tetanos": "tétanos",

    # Verbs in past 3rd person (rare)
    "llego": "llegó",

    # Imperfect tense verb forms (-ía ending)
    "habia": "había", "habian": "habían", "habias": "habías",
    "tenia": "tenía", "tenian": "tenían", "tenias": "tenías",
    "podia": "podía", "podian": "podían", "podias": "podías",
    "decia": "decía", "decian": "decían",
    "sabia": "sabía", "sabian": "sabían",
    "sabria": "sabría", "sabrian": "sabrían",
    "deberia": "debería", "deberian": "deberían",
    "concedian": "concedían", "concedia": "concedía",
    "creia": "creía", "creian": "creían",
    "veia": "veía", "veian": "veían",
    "queria": "quería", "querian": "querían",
    "ponia": "ponía", "ponian": "ponían",
    "sentia": "sentía", "sentian": "sentían",

    # Capitalized variants of already-listed words
    "Asimetria": "Asimetría",
    "Astenico": "Asténico", "Astenica": "Asténica",
    "Atletico": "Atlético", "Atletica": "Atlética",
    "Caracteristico": "Característico", "Caracteristica": "Característica",
    "Caracteristicos": "Característicos", "Caracteristicas": "Características",
    "Fisiopatologia": "Fisiopatología",
    "Galeria": "Galería", "Galerias": "Galerías",
    "Hipocratica": "Hipocrática", "Hipocratico": "Hipocrático",
    "Leptosomico": "Leptosómico", "Leptosomica": "Leptosómica",
    "Medica": "Médica", "Medico": "Médico",
    "Miastenica": "Miasténica", "Miastenico": "Miasténico",
    "Mioclonia": "Mioclonía", "Mioclonias": "Mioclonías",
    "Neurologia": "Neurología",
    "Patologia": "Patología", "Patologias": "Patologías",
    "Picnico": "Pícnico", "Picnica": "Pícnica",
    "Pletorica": "Pletórica", "Pletorico": "Pletórico",
    "Propedeutica": "Propedéutica", "Propedeutico": "Propedéutico",
    "Tecnica": "Técnica", "Tecnicas": "Técnicas",
    "Tecnico": "Técnico", "Tecnicos": "Técnicos",
    "Musculo": "Músculo", "Musculos": "Músculos",
    "Ultimo": "Último", "Unico": "Único",
    "Facil": "Fácil", "Util": "Útil",
    "Rapido": "Rápido", "Rigido": "Rígido",
    "Habitos": "Hábitos", "Metodos": "Métodos",
    "Hipotesis": "Hipótesis",
    "Sintomas": "Síntomas", "Sintoma": "Síntoma",
    "Craneo": "Cráneo", "Torax": "Tórax",
    "Cateter": "Catéter",
    "Organo": "Órgano", "Organos": "Órganos",
    "Oxigeno": "Oxígeno",
    "Perifericos": "Periféricos", "Periferico": "Periférico",
    "Periferica": "Periférica", "Perifericas": "Periféricas",
    "Magnetica": "Magnética", "Magnetico": "Magnético",
    "Cronico": "Crónico", "Cronica": "Crónica",
    "Toxico": "Tóxico", "Toxica": "Tóxica",
    "Cardiaco": "Cardíaco",
    "Sindromes": "Síndromes",
    "Nicolas": "Nicolás",

    # Specific missed items from text
    "sinnumero": "sinnúmero",
    "criptografico": "criptográfico", "criptografica": "criptográfica",
    "hispanico": "hispánico", "hispanica": "hispánica",
    "diciendoles": "diciéndoles",
    "percibais": "percibáis",
    "diagnostica": "diagnóstica", "diagnosticas": "diagnósticas",
    "enigmaticas": "enigmáticas", "enigmatico": "enigmático",
    "enigmatica": "enigmática",
    "dermico": "dérmico", "dermica": "dérmica",
    "retrocede": "retrocede",
    "alcoholica": "alcohólica", "alcoholico": "alcohólico",
    "termino": "término", "terminos": "términos",
    "cardiacos": "cardíacos",

    # Third pass: more missed words
    "Habito": "Hábito",
    "especifico": "específico", "especifica": "específica",
    "especificos": "específicos", "especificas": "específicas",
    "Especifico": "Específico", "Especifica": "Específica",
    "tabetico": "tabético", "tabetica": "tabética",
    "limpido": "límpido",
    "corazon": "corazón",
    "Corazon": "Corazón",
    "extrahepaticas": "extrahepáticas", "extrahepatico": "extrahepático",
    "paretica": "parética", "paretico": "parético",
    "Midriasis": "Midriasis",  # no change (already ok)
    "aun": "aún",  # ambiguous with "aun" (even); mostly means "todavía" in medical text
    "Aun": "Aún",
    # verb irregulars / past forms
    "duo": "dúo",
    "capiton": "capitón",

    # Other
    "sangre": "sangre",  # no change
    "aspera": "áspera", "aspero": "áspero",
    "asperas": "ásperas", "asperos": "ásperos",
    "palidos": "pálidos", "palido": "pálido",
    "palida": "pálida", "palidas": "pálidas",
}


def load(p):
    with open(p, encoding="utf-8") as f:
        return f.read()


def save(p, s):
    with open(p, "w", encoding="utf-8") as f:
        f.write(s)


def protect(text):
    """Stash attribute values and JS navigateTo calls so they survive intact."""
    stash = []

    def grab(m):
        stash.append(m.group(0))
        return f"\x00P{len(stash)-1}\x00"

    protect_patterns = [
        # HTML attributes whose values must not be transformed
        r'\b(?:id|href|data-section|class|src|for)="[^"]*"',
        r"\b(?:id|href|data-section|class|src|for)='[^']*'",
        # JS navigation calls
        r"navigateTo\(['\"][^'\"]*['\"]\)",
        # CSS selectors / Style values inside script? (not expected)
    ]
    for pat in protect_patterns:
        text = re.sub(pat, grab, text)
    return text, stash


def unprotect(text, stash):
    for i, val in enumerate(stash):
        text = text.replace(f"\x00P{i}\x00", val)
    return text


def apply_cion_sion(text):
    """Add accent to words ending in -cion/-sion: -> -ción/-sión."""
    text = re.sub(r"(?<=[A-Za-zÁÉÍÓÚáéíóúñÑ])cion\b", "ción", text)
    text = re.sub(r"(?<=[A-Za-zÁÉÍÓÚáéíóúñÑ])sion\b", "sión", text)
    return text


def apply_word_dict(text, d):
    # longest first
    for key in sorted(d, key=len, reverse=True):
        if key == d[key]:
            continue
        text = re.sub(r"\b" + re.escape(key) + r"\b", d[key], text)
    return text


def main():
    for path in FILES:
        before = load(path)
        text, stash = protect(before)
        text = apply_cion_sion(text)
        text = apply_word_dict(text, WORDS)
        text = unprotect(text, stash)
        if text != before:
            save(path, text)
            print(f"Updated: {path}")
        else:
            print(f"No change: {path}")


if __name__ == "__main__":
    main()
