$targetDir = "d:\SWE\StudentPortal"
$files = Get-ChildItem -Path $targetDir -Filter *.cs -Recurse

foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw
    $newContent = $content -replace 'attendance_management_system', 'EduTrackPro.StudentPortal'
    if ($content -ne $newContent) {
        Set-Content -Path $file.FullName -Value $newContent
        Write-Host "Updated $($file.FullName)"
    }
}
