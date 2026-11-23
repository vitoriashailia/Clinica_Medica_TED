const lista = document.querySelector('#lista');
const form = document.querySelector('#form-consulta');
const mensagem = document.querySelector('#mensagem');

async function carregarConsultas() {
  const parametros = new URLSearchParams({
    busca: document.querySelector('#busca').value,
    status: document.querySelector('#status').value,
    ordem: document.querySelector('#ordem').value
  });
  const resposta = await fetch(`/api/consultas?${parametros}`);
  const consultas = await resposta.json();
  lista.innerHTML = '';
  document.querySelector('#total').textContent = `${consultas.length} ${consultas.length === 1 ? 'consulta' : 'consultas'}`;
  document.querySelector('#vazio').hidden = consultas.length > 0;

  for (const consulta of consultas) {
    const linha = document.createElement('tr');
    const data = new Date(consulta.dataHora).toLocaleString('pt-BR');
    linha.innerHTML = `<td>${escapar(consulta.paciente)}</td><td>${escapar(consulta.medico)}</td><td>${escapar(consulta.especialidade || '—')}</td><td>${data}</td><td><span class="status-tag">${escapar(consulta.status)}</span></td><td><button class="excluir" data-id="${consulta.id}" aria-label="Excluir consulta">Excluir</button></td>`;
    lista.appendChild(linha);
  }
}

function escapar(valor) {
  const elemento = document.createElement('span');
  elemento.textContent = valor;
  return elemento.innerHTML;
}

form.addEventListener('submit', async evento => {
  evento.preventDefault();
  mensagem.textContent = '';
  const dados = Object.fromEntries(new FormData(form));
  dados.dataHora = new Date(dados.dataHora).toISOString();
  const resposta = await fetch('/api/consultas', {
    method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(dados)
  });
  if (!resposta.ok) {
    const erro = await resposta.json(); mensagem.textContent = erro.mensagem; return;
  }
  form.reset(); await carregarConsultas();
});

lista.addEventListener('click', async evento => {
  const botao = evento.target.closest('[data-id]');
  if (!botao || !confirm('Deseja excluir esta consulta?')) return;
  await fetch(`/api/consultas/${botao.dataset.id}`, { method: 'DELETE' });
  await carregarConsultas();
});

document.querySelector('#busca').addEventListener('input', carregarConsultas);
document.querySelector('#status').addEventListener('change', carregarConsultas);
document.querySelector('#ordem').addEventListener('change', carregarConsultas);
carregarConsultas();
