async function login(data) {
    const respuesta = await fetch('/api/auth/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data),
        credentials: 'include' // 🔥 IMPORTANT (cookies)
    });

    return respuesta.ok;
}

async function cerrarSesion(){
    const respuesta = await fetch('/api/auth/cerrarSesion', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' }
    })

    return respuesta.ok;
}