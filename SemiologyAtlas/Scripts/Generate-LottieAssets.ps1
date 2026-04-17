[CmdletBinding()]
param(
    [string]$OutputDir = ""
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

if ([string]::IsNullOrWhiteSpace($OutputDir))
{
    $OutputDir = Join-Path $PSScriptRoot "..\Resources\Raw"
}

function New-Color([string]$Hex)
{
    $clean = $Hex.TrimStart("#")
    if ($clean.Length -ne 6)
    {
        throw "Expected a 6-digit hex color. Got '$Hex'."
    }

    return @(
        [Math]::Round(([Convert]::ToInt32($clean.Substring(0, 2), 16) / 255.0), 4),
        [Math]::Round(([Convert]::ToInt32($clean.Substring(2, 2), 16) / 255.0), 4),
        [Math]::Round(([Convert]::ToInt32($clean.Substring(4, 2), 16) / 255.0), 4),
        1
    )
}

function New-StaticValue([object]$Value)
{
    return [ordered]@{
        a = 0
        k = $Value
    }
}

function Convert-ToKeyframeValue([object]$Value)
{
    if ($Value -is [System.Array])
    {
        return [object[]]$Value
    }

    return [object[]]@($Value)
}

function New-Step([int]$Time, [object]$Value)
{
    if ($Value -is [string])
    {
        $parsedInt = 0
        $parsedDouble = 0.0

        if ([int]::TryParse($Value, [ref]$parsedInt))
        {
            $Value = $parsedInt
        }
        elseif ([double]::TryParse($Value, [System.Globalization.NumberStyles]::Float, [System.Globalization.CultureInfo]::InvariantCulture, [ref]$parsedDouble))
        {
            $Value = $parsedDouble
        }
    }

    return [pscustomobject]@{
        t = $Time
        v = $Value
    }
}

function New-AnimatedValue([object[]]$Steps)
{
    $frames = @()

    for ($i = 0; $i -lt $Steps.Count; $i++)
    {
        $current = $Steps[$i]
        $startValue = @(Convert-ToKeyframeValue $current.v)
        $frame = [ordered]@{
            t = $current.t
            s = $startValue
        }

        if ($i -lt ($Steps.Count - 1))
        {
            $next = $Steps[$i + 1]
            $dimensions = $startValue.Count

            $frame.e = @(Convert-ToKeyframeValue $next.v)
            $frame.i = [ordered]@{
                x = @()
                y = @()
            }
            $frame.o = [ordered]@{
                x = @()
                y = @()
            }

            for ($axis = 0; $axis -lt $dimensions; $axis++)
            {
                $frame.i.x += 0.667
                $frame.i.y += 1
                $frame.o.x += 0.333
                $frame.o.y += 0
            }
        }

        $frames += $frame
    }

    return [ordered]@{
        a = 1
        k = $frames
    }
}

function Use-LottieValue([object]$Value)
{
    if ($Value -is [System.Collections.IDictionary] -and $Value.Contains("a"))
    {
        return $Value
    }

    if ($null -ne $Value -and $null -ne $Value.PSObject -and $Value.PSObject.Properties.Match("a").Count -gt 0)
    {
        return $Value
    }

    return New-StaticValue $Value
}

function New-Transform([object]$Position, [object]$Anchor, [object]$Scale = @(100, 100, 100), [object]$Rotation = 0, [object]$Opacity = 100)
{
    return [ordered]@{
        o = (Use-LottieValue $Opacity)
        r = (Use-LottieValue $Rotation)
        p = (Use-LottieValue $Position)
        a = (Use-LottieValue $Anchor)
        s = (Use-LottieValue $Scale)
    }
}

function New-ShapeTransform()
{
    return [ordered]@{
        ty = "tr"
        p = (New-StaticValue @(0, 0))
        a = (New-StaticValue @(0, 0))
        s = (New-StaticValue @(100, 100))
        r = (New-StaticValue 0)
        o = (New-StaticValue 100)
        sk = (New-StaticValue 0)
        sa = (New-StaticValue 0)
        nm = "Transform"
    }
}

function New-Stroke([object]$Color, [double]$Width, [string]$Name = "Stroke")
{
    return [ordered]@{
        ty = "st"
        c = (New-StaticValue $Color)
        o = (New-StaticValue 100)
        w = (New-StaticValue $Width)
        lc = 2
        lj = 2
        ml = 4
        nm = $Name
    }
}

function New-Fill([object]$Color, [string]$Name = "Fill")
{
    return [ordered]@{
        ty = "fl"
        c = (New-StaticValue $Color)
        o = (New-StaticValue 100)
        r = 1
        nm = $Name
    }
}

function New-PathValue([object[]]$Points, [bool]$Closed = $false)
{
    $inTangents = @()
    $outTangents = @()

    foreach ($point in $Points)
    {
        $inTangents += ,@(0, 0)
        $outTangents += ,@(0, 0)
    }

    return [ordered]@{
        i = $inTangents
        o = $outTangents
        v = $Points
        c = $Closed
    }
}

function New-PolylineGroup([string]$Name, [object[]]$Points, [object]$StrokeColor, [double]$StrokeWidth)
{
    return [ordered]@{
        ty = "gr"
        nm = $Name
        np = 2
        cix = 2
        bm = 0
        it = @(
            [ordered]@{
                ind = 0
                ty = "sh"
                ks = (New-StaticValue (New-PathValue -Points $Points))
                nm = "$Name Path"
            },
            (New-Stroke -Color $StrokeColor -Width $StrokeWidth -Name "$Name Stroke"),
            (New-ShapeTransform)
        )
    }
}

function New-CircleGroup([string]$Name, [double]$X, [double]$Y, [double]$Width, [double]$Height, [object]$FillColor = $null, [object]$StrokeColor = $null, [double]$StrokeWidth = 0)
{
    $items = @(
        [ordered]@{
            ty = "el"
            p = (New-StaticValue @($X, $Y))
            s = (New-StaticValue @($Width, $Height))
            nm = "$Name Ellipse"
        }
    )

    if ($null -ne $FillColor)
    {
        $items += ,(New-Fill -Color $FillColor -Name "$Name Fill")
    }

    if ($null -ne $StrokeColor)
    {
        $items += ,(New-Stroke -Color $StrokeColor -Width $StrokeWidth -Name "$Name Stroke")
    }

    $items += ,(New-ShapeTransform)

    return [ordered]@{
        ty = "gr"
        nm = $Name
        np = 2
        cix = 2
        bm = 0
        it = $items
    }
}

function New-RectGroup([string]$Name, [double]$X, [double]$Y, [double]$Width, [double]$Height, [double]$Radius, [object]$FillColor = $null, [object]$StrokeColor = $null, [double]$StrokeWidth = 0)
{
    $items = @(
        [ordered]@{
            ty = "rc"
            p = (New-StaticValue @($X, $Y))
            s = (New-StaticValue @($Width, $Height))
            r = (New-StaticValue $Radius)
            nm = "$Name Rect"
        }
    )

    if ($null -ne $FillColor)
    {
        $items += ,(New-Fill -Color $FillColor -Name "$Name Fill")
    }

    if ($null -ne $StrokeColor)
    {
        $items += ,(New-Stroke -Color $StrokeColor -Width $StrokeWidth -Name "$Name Stroke")
    }

    $items += ,(New-ShapeTransform)

    return [ordered]@{
        ty = "gr"
        nm = $Name
        np = 2
        cix = 2
        bm = 0
        it = $items
    }
}

function New-ShapeLayer([int]$Index, [string]$Name, [object]$Position, [object[]]$Shapes, [object]$Rotation = 0, [object]$Scale = @(100, 100, 100), [object]$Anchor = @(0, 0, 0), [object]$Opacity = 100)
{
    return [ordered]@{
        ddd = 0
        ind = $Index
        ty = 4
        nm = $Name
        sr = 1
        ks = (New-Transform -Position $Position -Anchor $Anchor -Scale $Scale -Rotation $Rotation -Opacity $Opacity)
        ao = 0
        shapes = $Shapes
        ip = 0
        op = 90
        st = 0
        bm = 0
    }
}

function New-LottieDocument([string]$Name, [object[]]$Layers)
{
    return [ordered]@{
        v = "5.7.4"
        fr = 30
        ip = 0
        op = 90
        w = 512
        h = 512
        nm = $Name
        ddd = 0
        assets = @()
        layers = $Layers
    }
}

function New-HeadLayer([int]$Index, [object]$Position, [object]$Rotation = 0, [object]$StrokeColor, [object]$FillColor)
{
    return New-ShapeLayer -Index $Index -Name "Head" -Position $Position -Rotation $Rotation -Shapes @(
        (New-CircleGroup -Name "Head" -X 0 -Y 0 -Width 68 -Height 68 -FillColor $FillColor -StrokeColor $StrokeColor -StrokeWidth 8),
        (New-PolylineGroup -Name "Smile" -Points @(@(-14, 12), @(0, 20), @(14, 12)) -StrokeColor $StrokeColor -StrokeWidth 5)
    )
}

function New-TorsoLayer([int]$Index, [object]$Position, [object]$Scale = @(100, 100, 100), [object]$Rotation = 0, [object]$StrokeColor, [object]$AccentColor)
{
    return New-ShapeLayer -Index $Index -Name "Torso" -Position $Position -Rotation $Rotation -Scale $Scale -Shapes @(
        (New-PolylineGroup -Name "Spine" -Points @(@(0, -26), @(0, 86)) -StrokeColor $StrokeColor -StrokeWidth 12),
        (New-PolylineGroup -Name "Shoulders" -Points @(@(-58, -12), @(58, -12)) -StrokeColor $StrokeColor -StrokeWidth 10),
        (New-PolylineGroup -Name "Hips" -Points @(@(-34, 82), @(34, 82)) -StrokeColor $StrokeColor -StrokeWidth 10),
        (New-CircleGroup -Name "Center" -X 0 -Y 26 -Width 24 -Height 24 -FillColor $AccentColor)
    )
}

function New-ChestLayer([int]$Index, [object]$Position, [object]$Scale, [object]$StrokeColor, [object]$AccentColor)
{
    return New-ShapeLayer -Index $Index -Name "Chest" -Position $Position -Scale $Scale -Shapes @(
        (New-CircleGroup -Name "LeftLung" -X -22 -Y 12 -Width 44 -Height 64 -StrokeColor $AccentColor -StrokeWidth 6),
        (New-CircleGroup -Name "RightLung" -X 22 -Y 12 -Width 44 -Height 64 -StrokeColor $AccentColor -StrokeWidth 6),
        (New-CircleGroup -Name "Heart" -X 6 -Y 18 -Width 20 -Height 20 -FillColor $StrokeColor)
    )
}

function New-AbdomenLayer([int]$Index, [object]$Position, [object]$Scale, [object]$StrokeColor, [object]$AccentColor)
{
    return New-ShapeLayer -Index $Index -Name "Abdomen" -Position $Position -Scale $Scale -Shapes @(
        (New-CircleGroup -Name "Belly" -X 0 -Y 0 -Width 108 -Height 86 -StrokeColor $StrokeColor -StrokeWidth 8),
        (New-CircleGroup -Name "LiverFocus" -X 26 -Y 10 -Width 24 -Height 24 -FillColor $AccentColor)
    )
}

function New-ArmLayer([int]$Index, [string]$Name, [object]$Position, [object]$Rotation, [object]$StrokeColor, [object]$HandColor, [double]$Length = 94)
{
    return New-ShapeLayer -Index $Index -Name $Name -Position $Position -Rotation $Rotation -Shapes @(
        (New-PolylineGroup -Name "$Name Arm" -Points @(@(0, 0), @(0, $Length)) -StrokeColor $StrokeColor -StrokeWidth 12),
        (New-CircleGroup -Name "$Name Hand" -X 0 -Y ($Length + 12) -Width 24 -Height 24 -FillColor $HandColor -StrokeColor $StrokeColor -StrokeWidth 6),
        (New-PolylineGroup -Name "$Name FingerLeft" -Points @(@(-6, ($Length + 20)), @(-10, ($Length + 34))) -StrokeColor $StrokeColor -StrokeWidth 4),
        (New-PolylineGroup -Name "$Name FingerCenter" -Points @(@(0, ($Length + 18)), @(0, ($Length + 38))) -StrokeColor $StrokeColor -StrokeWidth 4),
        (New-PolylineGroup -Name "$Name FingerRight" -Points @(@(6, ($Length + 20)), @(10, ($Length + 34))) -StrokeColor $StrokeColor -StrokeWidth 4)
    )
}

function New-ForearmLayer([int]$Index, [string]$Name, [object]$Position, [object]$Rotation, [object]$StrokeColor, [object]$HandColor, [double]$Length = 80)
{
    return New-ShapeLayer -Index $Index -Name $Name -Position $Position -Rotation $Rotation -Shapes @(
        (New-PolylineGroup -Name "$Name Forearm" -Points @(@(0, 0), @(0, $Length)) -StrokeColor $StrokeColor -StrokeWidth 12),
        (New-CircleGroup -Name "$Name Palm" -X 0 -Y ($Length + 8) -Width 28 -Height 24 -FillColor $HandColor -StrokeColor $StrokeColor -StrokeWidth 6),
        (New-PolylineGroup -Name "$Name Thumb" -Points @(@(8, ($Length + 2)), @(20, ($Length - 4))) -StrokeColor $StrokeColor -StrokeWidth 4),
        (New-PolylineGroup -Name "$Name Fingers" -Points @(@(-8, ($Length + 16)), @(-6, ($Length + 34)), @(0, ($Length + 38)), @(6, ($Length + 34)), @(8, ($Length + 16))) -StrokeColor $StrokeColor -StrokeWidth 4)
    )
}

function New-LegLayer([int]$Index, [string]$Name, [object]$Position, [object]$Rotation, [object]$StrokeColor, [double]$Length = 112)
{
    return New-ShapeLayer -Index $Index -Name $Name -Position $Position -Rotation $Rotation -Shapes @(
        (New-PolylineGroup -Name "$Name Leg" -Points @(@(0, 0), @(0, $Length)) -StrokeColor $StrokeColor -StrokeWidth 12),
        (New-PolylineGroup -Name "$Name Foot" -Points @(@(0, $Length), @(22, ($Length + 6))) -StrokeColor $StrokeColor -StrokeWidth 8)
    )
}

function New-ScanRingLayer([int]$Index, [object]$Position, [object]$Scale, [object]$StrokeColor)
{
    return New-ShapeLayer -Index $Index -Name "ScanRing" -Position $Position -Scale $Scale -Opacity (New-AnimatedValue @(
        (New-Step 0 0),
        (New-Step 10 80),
        (New-Step 80 80),
        (New-Step 90 0)
    )) -Shapes @(
        (New-CircleGroup -Name "Scan" -X 0 -Y 0 -Width 168 -Height 86 -StrokeColor $StrokeColor -StrokeWidth 8)
    )
}

function New-ClipboardLayer([int]$Index, [object]$Position, [object]$Rotation, [object]$StrokeColor, [object]$FillColor)
{
    return New-ShapeLayer -Index $Index -Name "Clipboard" -Position $Position -Rotation $Rotation -Shapes @(
        (New-RectGroup -Name "Board" -X 0 -Y 0 -Width 56 -Height 74 -Radius 10 -FillColor $FillColor -StrokeColor $StrokeColor -StrokeWidth 6),
        (New-RectGroup -Name "Clip" -X 0 -Y (-34) -Width 20 -Height 12 -Radius 4 -FillColor $StrokeColor)
    )
}

function New-StethoscopeLayer([int]$Index, [object]$Position, [object]$StrokeColor)
{
    return New-ShapeLayer -Index $Index -Name "Stethoscope" -Position $Position -Shapes @(
        (New-PolylineGroup -Name "Tube" -Points @(@(-22, -44), @(-22, -16), @(0, 10), @(22, -16), @(22, -44)) -StrokeColor $StrokeColor -StrokeWidth 6),
        (New-CircleGroup -Name "Bell" -X 0 -Y 22 -Width 20 -Height 20 -StrokeColor $StrokeColor -StrokeWidth 6),
        (New-PolylineGroup -Name "EarsLeft" -Points @(@(-32, -50), @(-22, -58), @(-12, -50)) -StrokeColor $StrokeColor -StrokeWidth 4),
        (New-PolylineGroup -Name "EarsRight" -Points @(@(12, -50), @(22, -58), @(32, -50)) -StrokeColor $StrokeColor -StrokeWidth 4)
    )
}

function New-HammerLayer([int]$Index, [object]$Position, [object]$Rotation, [object]$StrokeColor, [object]$FillColor)
{
    return New-ShapeLayer -Index $Index -Name "Hammer" -Position $Position -Rotation $Rotation -Shapes @(
        (New-PolylineGroup -Name "Handle" -Points @(@(0, 18), @(0, 78)) -StrokeColor $StrokeColor -StrokeWidth 8),
        (New-RectGroup -Name "Head" -X 0 -Y 0 -Width 46 -Height 18 -Radius 8 -FillColor $FillColor -StrokeColor $StrokeColor -StrokeWidth 6)
    )
}

$outline = New-Color "#173F5F"
$accent = New-Color "#E07A4F"
$teal = New-Color "#2B7A78"
$gold = New-Color "#F2C14E"
$sand = New-Color "#F5EFE6"

$shortSwing = New-AnimatedValue @(
    (New-Step 0 -8),
    (New-Step 18 6),
    (New-Step 36 -4),
    (New-Step 54 4),
    (New-Step 72 -2),
    (New-Step 90 2)
)
$restTremor = New-AnimatedValue @(
    (New-Step 0 -8),
    (New-Step 10 8),
    (New-Step 20 -7),
    (New-Step 30 7),
    (New-Step 40 -8),
    (New-Step 50 8),
    (New-Step 60 -7),
    (New-Step 70 7),
    (New-Step 80 -8),
    (New-Step 90 8)
)
$essentialTremor = New-AnimatedValue @(
    (New-Step 0 -10),
    (New-Step 8 10),
    (New-Step 16 -10),
    (New-Step 24 10),
    (New-Step 32 -10),
    (New-Step 40 10),
    (New-Step 48 -10),
    (New-Step 56 10),
    (New-Step 64 -10),
    (New-Step 72 10),
    (New-Step 80 -10),
    (New-Step 90 10)
)
$irregularMovement = New-AnimatedValue @(
    (New-Step 0 -12),
    (New-Step 10 18),
    (New-Step 24 -4),
    (New-Step 34 22),
    (New-Step 48 -18),
    (New-Step 62 12),
    (New-Step 76 -6),
    (New-Step 90 16)
)
$jerkMovement = New-AnimatedValue @(
    (New-Step 0 0),
    (New-Step 12 0),
    (New-Step 16 -26),
    (New-Step 20 2),
    (New-Step 38 2),
    (New-Step 42 26),
    (New-Step 46 0),
    (New-Step 68 0),
    (New-Step 72 -24),
    (New-Step 76 4),
    (New-Step 90 0)
)
$scanPosition = New-AnimatedValue @(
    (New-Step 0 @(256, 120, 0)),
    (New-Step 45 @(256, 240, 0)),
    (New-Step 90 @(256, 360, 0))
)
$stethoscopeCardio = New-AnimatedValue @(
    (New-Step 0 @(256, 146, 0)),
    (New-Step 24 @(232, 212, 0)),
    (New-Step 52 @(280, 224, 0)),
    (New-Step 90 @(252, 196, 0))
)
$stethoscopeResp = New-AnimatedValue @(
    (New-Step 0 @(236, 220, 0)),
    (New-Step 30 @(274, 220, 0)),
    (New-Step 60 @(236, 232, 0)),
    (New-Step 90 @(274, 232, 0))
)
$chestScale = New-AnimatedValue @(
    (New-Step 0 @(100, 100, 100)),
    (New-Step 24 @(106, 118, 100)),
    (New-Step 48 @(100, 100, 100)),
    (New-Step 72 @(106, 118, 100)),
    (New-Step 90 @(100, 100, 100))
)
$abdomenScale = New-AnimatedValue @(
    (New-Step 0 @(100, 100, 100)),
    (New-Step 28 @(108, 112, 100)),
    (New-Step 58 @(100, 100, 100)),
    (New-Step 90 @(108, 112, 100))
)
$palpationHands = New-AnimatedValue @(
    (New-Step 0 @(256, 286, 0)),
    (New-Step 24 @(256, 306, 0)),
    (New-Step 48 @(256, 286, 0)),
    (New-Step 72 @(256, 306, 0)),
    (New-Step 90 @(256, 286, 0))
)
$hammerMotion = New-AnimatedValue @(
    (New-Step 0 @(334, 318, 0)),
    (New-Step 18 @(314, 344, 0)),
    (New-Step 36 @(334, 318, 0)),
    (New-Step 54 @(314, 344, 0)),
    (New-Step 72 @(334, 318, 0)),
    (New-Step 90 @(314, 344, 0))
)
$bradySequence = New-AnimatedValue @(
    (New-Step 0 -16),
    (New-Step 12 16),
    (New-Step 24 -10),
    (New-Step 36 10),
    (New-Step 48 -6),
    (New-Step 60 6),
    (New-Step 72 -3),
    (New-Step 90 3)
)
$rigiditySweep = New-AnimatedValue @(
    (New-Step 0 -18),
    (New-Step 30 18),
    (New-Step 60 -18),
    (New-Step 90 18)
)
$dystoniaHead = New-AnimatedValue @(
    (New-Step 0 0),
    (New-Step 24 -16),
    (New-Step 56 -24),
    (New-Step 90 -24)
)
$dystoniaArm = New-AnimatedValue @(
    (New-Step 0 6),
    (New-Step 24 34),
    (New-Step 56 48),
    (New-Step 90 48)
)
$parkinsonGaitLegLeft = New-AnimatedValue @(
    (New-Step 0 -10),
    (New-Step 24 8),
    (New-Step 48 -8),
    (New-Step 72 8),
    (New-Step 90 -10)
)
$parkinsonGaitLegRight = New-AnimatedValue @(
    (New-Step 0 8),
    (New-Step 24 -10),
    (New-Step 48 8),
    (New-Step 72 -8),
    (New-Step 90 8)
)
$cerebellarLegLeft = New-AnimatedValue @(
    (New-Step 0 -18),
    (New-Step 24 16),
    (New-Step 48 -14),
    (New-Step 72 16),
    (New-Step 90 -18)
)
$cerebellarLegRight = New-AnimatedValue @(
    (New-Step 0 16),
    (New-Step 24 -18),
    (New-Step 48 16),
    (New-Step 72 -14),
    (New-Step 90 16)
)
$trunkSway = New-AnimatedValue @(
    (New-Step 0 @(256, 216, 0)),
    (New-Step 24 @(240, 216, 0)),
    (New-Step 48 @(270, 216, 0)),
    (New-Step 72 @(242, 216, 0)),
    (New-Step 90 @(256, 216, 0))
)

$assets = @(
    [pscustomobject]@{
        Key = "general_survey"
        Layers = @(
            (New-ShapeLayer -Index 1 -Name "ClinicianBody" -Position @(146, 224, 0) -Shapes @(
                (New-CircleGroup -Name "ClinicianHead" -X 0 -Y -82 -Width 56 -Height 56 -FillColor $sand -StrokeColor $outline -StrokeWidth 8),
                (New-PolylineGroup -Name "ClinicianSpine" -Points @(@(0, -50), @(0, 40)) -StrokeColor $outline -StrokeWidth 10),
                (New-PolylineGroup -Name "ClinicianArm" -Points @(@(0, -18), @(28, 8)) -StrokeColor $outline -StrokeWidth 8)
            )),
            (New-ClipboardLayer -Index 2 -Position @(182, 238, 0) -Rotation -10 -StrokeColor $outline -FillColor $gold),
            (New-HeadLayer -Index 3 -Position @(256, 122, 0) -StrokeColor $outline -FillColor $sand),
            (New-TorsoLayer -Index 4 -Position @(256, 216, 0) -StrokeColor $outline -AccentColor $teal),
            (New-ArmLayer -Index 5 -Name "LeftArm" -Position @(198, 206, 0) -Rotation 8 -StrokeColor $outline -HandColor $sand),
            (New-ArmLayer -Index 6 -Name "RightArm" -Position @(314, 206, 0) -Rotation -8 -StrokeColor $outline -HandColor $sand),
            (New-LegLayer -Index 7 -Name "LeftLeg" -Position @(228, 300, 0) -Rotation 2 -StrokeColor $outline),
            (New-LegLayer -Index 8 -Name "RightLeg" -Position @(284, 300, 0) -Rotation -2 -StrokeColor $outline),
            (New-ScanRingLayer -Index 9 -Position $scanPosition -Scale @(100, 100, 100) -StrokeColor $accent)
        )
    },
    [pscustomobject]@{
        Key = "neurologic_screen"
        Layers = @(
            (New-HeadLayer -Index 1 -Position @(256, 118, 0) -StrokeColor $outline -FillColor $sand),
            (New-TorsoLayer -Index 2 -Position @(256, 214, 0) -StrokeColor $outline -AccentColor $accent),
            (New-ForearmLayer -Index 3 -Name "TapHand" -Position @(332, 174, 0) -Rotation $bradySequence -StrokeColor $outline -HandColor $sand),
            (New-ArmLayer -Index 4 -Name "LeftArm" -Position @(194, 206, 0) -Rotation 6 -StrokeColor $outline -HandColor $sand),
            (New-LegLayer -Index 5 -Name "LeftLeg" -Position @(228, 300, 0) -Rotation 2 -StrokeColor $outline),
            (New-LegLayer -Index 6 -Name "RightLeg" -Position @(284, 300, 0) -Rotation -2 -StrokeColor $outline),
            (New-HammerLayer -Index 7 -Position $hammerMotion -Rotation 28 -StrokeColor $outline -FillColor $gold)
        )
    },
    [pscustomobject]@{
        Key = "cardio_clues"
        Layers = @(
            (New-HeadLayer -Index 1 -Position @(256, 110, 0) -StrokeColor $outline -FillColor $sand),
            (New-TorsoLayer -Index 2 -Position @(256, 220, 0) -StrokeColor $outline -AccentColor $accent),
            (New-ChestLayer -Index 3 -Position @(256, 220, 0) -Scale @(100, 100, 100) -StrokeColor $outline -AccentColor $teal),
            (New-ShapeLayer -Index 4 -Name "JvpLeft" -Position @(0, 0, 0) -Shapes @(
                (New-PolylineGroup -Name "JvpLeftLine" -Points @(@(238, 146), @(228, 184)) -StrokeColor $accent -StrokeWidth 6)
            )),
            (New-ShapeLayer -Index 5 -Name "JvpRight" -Position @(0, 0, 0) -Shapes @(
                (New-PolylineGroup -Name "JvpRightLine" -Points @(@(274, 146), @(284, 184)) -StrokeColor $accent -StrokeWidth 6)
            )),
            (New-StethoscopeLayer -Index 6 -Position $stethoscopeCardio -StrokeColor $gold)
        )
    },
    [pscustomobject]@{
        Key = "resp_pattern"
        Layers = @(
            (New-HeadLayer -Index 1 -Position @(256, 110, 0) -Rotation 6 -StrokeColor $outline -FillColor $sand),
            (New-TorsoLayer -Index 2 -Position @(256, 220, 0) -Scale @(104, 100, 100) -Rotation 4 -StrokeColor $outline -AccentColor $accent),
            (New-ChestLayer -Index 3 -Position @(256, 220, 0) -Scale $chestScale -StrokeColor $outline -AccentColor $teal),
            (New-ShapeLayer -Index 4 -Name "NeckEffort" -Position @(256, 158, 0) -Opacity (New-AnimatedValue @(
                (New-Step 0 20),
                (New-Step 24 100),
                (New-Step 48 20),
                (New-Step 72 100),
                (New-Step 90 20)
            )) -Shapes @(
                (New-PolylineGroup -Name "AccessoryLeft" -Points @(@(-18, -6), @(-32, 26)) -StrokeColor $accent -StrokeWidth 6),
                (New-PolylineGroup -Name "AccessoryRight" -Points @(@(18, -6), @(32, 26)) -StrokeColor $accent -StrokeWidth 6)
            )),
            (New-StethoscopeLayer -Index 5 -Position $stethoscopeResp -StrokeColor $gold)
        )
    },
    [pscustomobject]@{
        Key = "gi_survey"
        Layers = @(
            (New-HeadLayer -Index 1 -Position @(256, 108, 0) -StrokeColor $outline -FillColor $sand),
            (New-TorsoLayer -Index 2 -Position @(256, 210, 0) -StrokeColor $outline -AccentColor $teal),
            (New-AbdomenLayer -Index 3 -Position @(256, 294, 0) -Scale $abdomenScale -StrokeColor $outline -AccentColor $accent),
            (New-ShapeLayer -Index 4 -Name "PalpationHands" -Position $palpationHands -Shapes @(
                (New-CircleGroup -Name "LeftHand" -X -42 -Y 0 -Width 44 -Height 24 -FillColor $gold -StrokeColor $outline -StrokeWidth 5),
                (New-CircleGroup -Name "RightHand" -X 42 -Y 0 -Width 44 -Height 24 -FillColor $gold -StrokeColor $outline -StrokeWidth 5)
            ))
        )
    },
    [pscustomobject]@{
        Key = "bradykinesia"
        Layers = @(
            (New-ForearmLayer -Index 1 -Name "BradyHand" -Position @(256, 170, 0) -Rotation $bradySequence -StrokeColor $outline -HandColor $sand),
            (New-ShapeLayer -Index 2 -Name "TargetPalm" -Position @(310, 328, 0) -Opacity (New-AnimatedValue @(
                (New-Step 0 100),
                (New-Step 24 80),
                (New-Step 48 65),
                (New-Step 72 50),
                (New-Step 90 40)
            )) -Shapes @(
                (New-CircleGroup -Name "Prompt" -X 0 -Y 0 -Width 30 -Height 30 -FillColor $accent)
            ))
        )
    },
    [pscustomobject]@{
        Key = "rigidity"
        Layers = @(
            (New-ForearmLayer -Index 1 -Name "RigidityArm" -Position @(246, 164, 0) -Rotation $rigiditySweep -StrokeColor $outline -HandColor $sand),
            (New-ShapeLayer -Index 2 -Name "ExaminerHand" -Position @(312, 278, 0) -Rotation -18 -Shapes @(
                (New-CircleGroup -Name "Palm" -X 0 -Y 0 -Width 52 -Height 26 -FillColor $gold -StrokeColor $outline -StrokeWidth 6)
            )),
            (New-ShapeLayer -Index 3 -Name "Joint" -Position @(246, 164, 0) -Shapes @(
                (New-CircleGroup -Name "Elbow" -X 0 -Y 0 -Width 22 -Height 22 -FillColor $accent)
            ))
        )
    },
    [pscustomobject]@{
        Key = "parkinson_tremor"
        Layers = @(
            (New-ShapeLayer -Index 1 -Name "Thigh" -Position @(232, 174, 0) -Shapes @(
                (New-PolylineGroup -Name "ThighLine" -Points @(@(0, 0), @(0, 168)) -StrokeColor $outline -StrokeWidth 18)
            )),
            (New-ForearmLayer -Index 2 -Name "RestHand" -Position @(286, 210, 0) -Rotation $restTremor -StrokeColor $outline -HandColor $sand)
        )
    },
    [pscustomobject]@{
        Key = "essential_tremor"
        Layers = @(
            (New-HeadLayer -Index 1 -Position @(256, 112, 0) -StrokeColor $outline -FillColor $sand),
            (New-TorsoLayer -Index 2 -Position @(256, 214, 0) -StrokeColor $outline -AccentColor $teal),
            (New-ArmLayer -Index 3 -Name "LeftOutstretch" -Position @(198, 204, 0) -Rotation $essentialTremor -StrokeColor $outline -HandColor $sand -Length 104),
            (New-ArmLayer -Index 4 -Name "RightOutstretch" -Position @(314, 204, 0) -Rotation $essentialTremor -StrokeColor $outline -HandColor $sand -Length 104)
        )
    },
    [pscustomobject]@{
        Key = "chorea"
        Layers = @(
            (New-HeadLayer -Index 1 -Position @(256, 112, 0) -Rotation $irregularMovement -StrokeColor $outline -FillColor $sand),
            (New-TorsoLayer -Index 2 -Position @(256, 214, 0) -Rotation 4 -StrokeColor $outline -AccentColor $accent),
            (New-ArmLayer -Index 3 -Name "RightArm" -Position @(314, 204, 0) -Rotation $irregularMovement -StrokeColor $outline -HandColor $sand),
            (New-LegLayer -Index 4 -Name "LeftLeg" -Position @(228, 302, 0) -Rotation $irregularMovement -StrokeColor $outline),
            (New-LegLayer -Index 5 -Name "RightLeg" -Position @(284, 302, 0) -Rotation (New-AnimatedValue @(
                (New-Step 0 6),
                (New-Step 18 -10),
                (New-Step 34 14),
                (New-Step 52 -4),
                (New-Step 72 12),
                (New-Step 90 -8)
            )) -StrokeColor $outline)
        )
    },
    [pscustomobject]@{
        Key = "dystonia"
        Layers = @(
            (New-HeadLayer -Index 1 -Position @(256, 110, 0) -Rotation $dystoniaHead -StrokeColor $outline -FillColor $sand),
            (New-TorsoLayer -Index 2 -Position @(256, 214, 0) -Rotation 6 -StrokeColor $outline -AccentColor $accent),
            (New-ArmLayer -Index 3 -Name "TwistedArm" -Position @(314, 204, 0) -Rotation $dystoniaArm -StrokeColor $outline -HandColor $sand),
            (New-ArmLayer -Index 4 -Name "CounterArm" -Position @(198, 204, 0) -Rotation -10 -StrokeColor $outline -HandColor $sand)
        )
    },
    [pscustomobject]@{
        Key = "myoclonus"
        Layers = @(
            (New-HeadLayer -Index 1 -Position @(256, 112, 0) -StrokeColor $outline -FillColor $sand),
            (New-TorsoLayer -Index 2 -Position @(256, 214, 0) -StrokeColor $outline -AccentColor $teal),
            (New-ArmLayer -Index 3 -Name "JerkArm" -Position @(314, 204, 0) -Rotation $jerkMovement -StrokeColor $outline -HandColor $sand),
            (New-ShapeLayer -Index 4 -Name "JerkPulse" -Position @(374, 288, 0) -Opacity (New-AnimatedValue @(
                (New-Step 0 0),
                (New-Step 16 100),
                (New-Step 20 0),
                (New-Step 42 100),
                (New-Step 46 0),
                (New-Step 72 100),
                (New-Step 76 0),
                (New-Step 90 0)
            )) -Shapes @(
                (New-CircleGroup -Name "PulseOuter" -X 0 -Y 0 -Width 44 -Height 44 -StrokeColor $accent -StrokeWidth 6)
            ))
        )
    },
    [pscustomobject]@{
        Key = "parkinson_gait"
        Layers = @(
            (New-HeadLayer -Index 1 -Position @(256, 98, 0) -StrokeColor $outline -FillColor $sand),
            (New-TorsoLayer -Index 2 -Position @(256, 190, 0) -Rotation 8 -StrokeColor $outline -AccentColor $accent),
            (New-ArmLayer -Index 3 -Name "LeftArm" -Position @(198, 182, 0) -Rotation $shortSwing -StrokeColor $outline -HandColor $sand),
            (New-ArmLayer -Index 4 -Name "RightArm" -Position @(314, 182, 0) -Rotation $shortSwing -StrokeColor $outline -HandColor $sand),
            (New-LegLayer -Index 5 -Name "LeftLeg" -Position @(228, 274, 0) -Rotation $parkinsonGaitLegLeft -StrokeColor $outline),
            (New-LegLayer -Index 6 -Name "RightLeg" -Position @(284, 274, 0) -Rotation $parkinsonGaitLegRight -StrokeColor $outline)
        )
    },
    [pscustomobject]@{
        Key = "cerebellar_gait"
        Layers = @(
            (New-HeadLayer -Index 1 -Position $trunkSway -StrokeColor $outline -FillColor $sand),
            (New-TorsoLayer -Index 2 -Position $trunkSway -Rotation (New-AnimatedValue @(
                (New-Step 0 -8),
                (New-Step 24 10),
                (New-Step 48 -10),
                (New-Step 72 8),
                (New-Step 90 -8)
            )) -StrokeColor $outline -AccentColor $teal),
            (New-ArmLayer -Index 3 -Name "LeftArm" -Position @(190, 192, 0) -Rotation -18 -StrokeColor $outline -HandColor $sand),
            (New-ArmLayer -Index 4 -Name "RightArm" -Position @(322, 192, 0) -Rotation 18 -StrokeColor $outline -HandColor $sand),
            (New-LegLayer -Index 5 -Name "LeftLeg" -Position @(206, 276, 0) -Rotation $cerebellarLegLeft -StrokeColor $outline),
            (New-LegLayer -Index 6 -Name "RightLeg" -Position @(306, 276, 0) -Rotation $cerebellarLegRight -StrokeColor $outline)
        )
    }
)

New-Item -ItemType Directory -Path $OutputDir -Force | Out-Null

foreach ($asset in $assets)
{
    $document = New-LottieDocument -Name $asset.Key -Layers $asset.Layers
    $json = $document | ConvertTo-Json -Depth 100
    $path = Join-Path $OutputDir "$($asset.Key).json"
    Set-Content -Path $path -Value $json -Encoding UTF8
}
