window.downloadFile = (nombre, typo, contenido) => {
    const archivo = new Blob([contenido], {type: typo});
    const anchor = document.createElement('a');

    anchor.href = URL.createObjectURL(archivo);
    anchor.download = nombre;
    anchor.click();

    URL.revokeObjectURL(anchor.href);
}