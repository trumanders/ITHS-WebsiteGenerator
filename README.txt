/// <summary>
    /// Programmet låter användaren mata in ett antal parametrar/innehåll till en websida.
    /// En HTML-fil genreras utifrån vad användaren matade in.
    /// Förklaring till menyvalen:
    /// 1. Skapa websida - lägger in en websida av typen SchoolWebsite i en List med alla websidor.
    /// 2. Skapa stylad websida - lägger in en websida av typen StyledSchoolWebsite i en List med alla websidor.
    /// 3. Visa alla nya genererade - visar innehållet i de genererade websidorna i aktuell session (ej sparade på disk)
    /// 4. Spara alla websidor till fil - sparar alla genererade websidor i separata filer på hårddisken, och lägger in
    ///     filnamnen i en txt-fil. Listorna rensas.
    /// 5. Lista sparade - listar alla HTML-filer som sparats på hårddisken. Programmet kollar först så att filerna verkligen ligger
    ///     på hårddisken och uppdaterar txt-filen med de sparade filerna.
    /// 6. Se innehåll i sparade - visar HTML-koden från alla filer som finns sparade på hårddisken.
/// </summary>