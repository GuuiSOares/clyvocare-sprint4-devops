# Sprint 4 — DevOps Tools & Cloud Computing

Entrega DevOps do **ClyvoCare** (API .NET + Oracle FIAP + Azure DevOps CI/CD).

O README da disciplina de .NET fica em [`ChallengeNET/README.md`](ChallengeNET/README.md). Este arquivo na raiz cobre **somente** a Sprint 4 de DevOps.

---

## 1. Descrição da solução

A aplicação **ClyvoCare** é uma API REST em **.NET 8 (ASP.NET Core)** para monitoramento de saúde animal (tutores, pets e logs de saúde), com CRUD e autenticação JWT.

### Stack utilizada (DevOps / nuvem)

| Camada | Tecnologia |
|--------|------------|
| Aplicação | .NET 8 / ASP.NET Core Web API |
| Banco em nuvem | Oracle Database (Oracle FIAP) |
| Controle de versão | GitHub |
| CI/CD | Azure DevOps Pipelines |
| Deploy | Azure Web App |
| Segredos | Variáveis protegidas do Azure DevOps (connection string, JWT) |
| Testes no CI | xUnit (unitários + integração) |

---

## 2. Diagrama da Arquitetura + Fluxo CI/CD

> **Status:** diagrama será incluído aqui (com ícones, personas, componentes da stack e etapas numeradas do CI/CD), conforme requisito da Sprint 4 e feedback da Sprint 3.

`docs/diagrama-arquitetura-cicd.png` *(adicionar após criar o desenho)*

### Personas
- **Desenvolvedor:** altera código, faz push na `main`
- **Azure DevOps:** executa CI/CD
- **Usuário final:** consome a API no Azure Web App
- **Professor:** acessa o projeto Azure DevOps (Basic) e o vídeo

### Fluxo CI/CD (ordem)

1. Push na branch `main` (GitHub)
2. Trigger automático da Pipeline (Azure DevOps)
3. **CI:** restore → build → testes → publicar artefato
4. **CD:** configura variáveis no Web App → deploy do artefato
5. Aplicação atualizada no **Azure Web App**, conectada ao **Oracle FIAP**

---

## 3. Banco de Dados em Nuvem

- **Tipo:** Oracle (serviço Oracle da FIAP) — permitido no edital
- **Uso na app:** Entity Framework Core (`ConnectionStrings:OracleConnection`)
- **Segredo:** connection string **não** fica no repositório; vai nas variáveis protegidas da pipeline / App Settings do Web App

---

## 4. Azure DevOps

| Campo | Valor |
|-------|--------|
| Project Name | `Sprint 4 – Azure DevOps` |
| Description | Projeto para entrega da Sprint 4 do professor *(preencher nome)* |
| Integrantes | RM562673 - Geovanne Coneglian Passos; RM563960 - Lucas Silva Gastão Pinheiro; RM563143 - Guilherme Soares De Almeida |
| Visibility | Private |
| Version control | Git (código no GitHub; pipeline conectada ao repo) |
| Work item process | Scrum ou Agile |
| Acesso professor | Convite com nível **Basic** |

**Links (preencher):**
- GitHub: https://github.com/GuuiSOares/clyvocare-sprint4-devops
- Azure DevOps: *(colar depois de criar)*
- YouTube (vídeo): *(colar depois de gravar)*

---

## 5. Pipelines CI/CD

- Modo: YAML (padrão das aulas — Semana 19 / Pricewhisper)
- Trigger: push na branch **`main`**
- **CI:** build + testes automáticos + publicação de artefato
- **CD:** deploy automático no **Azure Web App** após artefato
- Variáveis sensíveis: `OracleConnection`, `Jwt__Key`, etc. (secret/protegidas)

Arquivo da pipeline: `azure-pipelines.yml` *(será adicionado no próximo passo de infra/pipeline)*

---

## 6. Integrantes

| Nome | RM |
|------|-----|
| Geovanne Coneglian Passos | 562673 |
| Lucas Silva Gastão Pinheiro | 563960 |
| Guilherme Soares De Almeida | 563143 |
