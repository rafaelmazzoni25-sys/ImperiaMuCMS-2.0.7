(() => {
  const licenseTypes = ["Lite", "Bronze", "Silver", "Gold", "Premium", "PremiumPlus"];
  const licenseStatuses = ["Active", "Inactive", "Banned"];
  let authToken = localStorage.getItem("imperia-license-token") ?? "";
  let currentLicenses = [];

  const statusMessage = document.getElementById("status-message");
  const loginSection = document.getElementById("login-section");
  const dashboardSection = document.getElementById("dashboard-section");
  const logoutButton = document.getElementById("logout-button");
  const loginForm = document.getElementById("login-form");
  const licenseForm = document.getElementById("license-form");
  const licenseIdInput = document.getElementById("license-id");
  const licenseKeyInput = document.getElementById("license-key");
  const licenseEmailInput = document.getElementById("license-email");
  const licenseTypeSelect = document.getElementById("license-type");
  const licenseStatusSelect = document.getElementById("license-status");
  const licenseExpiresInput = document.getElementById("license-expires");
  const expirationEnabledCheckbox = document.getElementById("license-expiration-enabled");
  const licenseNotesInput = document.getElementById("license-notes");
  const licensesBody = document.getElementById("licenses-body");
  const refreshButton = document.getElementById("refresh-button");
  const resetButton = document.getElementById("reset-button");
  const refreshAuditButton = document.getElementById("refresh-audit");
  const auditList = document.getElementById("audit-list");
  const formTitle = document.getElementById("form-title");
  const saveButton = document.getElementById("save-button");

  function showMessage(message, type = "info") {
    statusMessage.textContent = message;
    statusMessage.style.color = type === "error" ? "#fecaca" : "#e2e8f0";
  }

  function toggleDashboard(visible) {
    if (visible) {
      loginSection.classList.add("hidden");
      dashboardSection.classList.remove("hidden");
      logoutButton.classList.remove("hidden");
    } else {
      loginSection.classList.remove("hidden");
      dashboardSection.classList.add("hidden");
      logoutButton.classList.add("hidden");
    }
  }

  async function apiFetch(url, options = {}) {
    const opts = { ...options };
    opts.headers = opts.headers ? { ...opts.headers } : {};
    if (authToken) {
      opts.headers["X-Auth-Token"] = authToken;
    }

    const response = await fetch(url, opts);
    if (response.status === 401) {
      handleUnauthorized();
      let message = "Autenticação obrigatória.";
      try {
        const payload = await response.json();
        if (payload?.message) {
          message = payload.message;
        }
      } catch (error) {
        // ignore json parse failures
      }
      throw new Error(message);
    }

    if (!response.ok) {
      let message = "Não foi possível concluir a operação.";
      try {
        const payload = await response.json();
        if (payload?.message) {
          message = payload.message;
        }
      } catch (error) {
        // ignore json parse failures
      }
      throw new Error(message);
    }

    if (response.status === 204) {
      return null;
    }

    const text = await response.text();
    return text ? JSON.parse(text) : null;
  }

  function handleUnauthorized() {
    authToken = "";
    localStorage.removeItem("imperia-license-token");
    toggleDashboard(false);
    showMessage("Sessão expirada. Faça login novamente.", "error");
  }

  async function login(event) {
    event.preventDefault();
    const formData = new FormData(loginForm);
    try {
      const result = await apiFetch("/admin/api/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          username: formData.get("username"),
          password: formData.get("password")
        })
      });

      authToken = result.token;
      localStorage.setItem("imperia-license-token", authToken);
      toggleDashboard(true);
      showMessage(`Bem-vindo, ${result.username}!`);
      loginForm.reset();
      await refreshData();
    } catch (error) {
      showMessage(error.message, "error");
    }
  }

  async function logout() {
    try {
      await apiFetch("/admin/api/logout", { method: "POST" });
    } catch (error) {
      console.warn(error);
    } finally {
      authToken = "";
      localStorage.removeItem("imperia-license-token");
      toggleDashboard(false);
      showMessage("Sessão encerrada com sucesso.");
    }
  }

  function resetLicenseForm() {
    licenseForm.reset();
    licenseIdInput.value = "";
    formTitle.textContent = "Cadastrar licença";
    saveButton.textContent = "Salvar";
    expirationEnabledCheckbox.checked = true;
    licenseExpiresInput.disabled = false;
    const defaultDate = new Date();
    defaultDate.setMonth(defaultDate.getMonth() + 1);
    licenseExpiresInput.value = defaultDate.toISOString().slice(0, 16);
    licenseNotesInput.value = "";
    licenseStatusSelect.value = "Active";
    licenseTypeSelect.value = "Premium";
  }

  function populateSelectOptions() {
    licenseStatusSelect.innerHTML = "";
    licenseStatuses.forEach(status => {
      const option = document.createElement("option");
      option.value = status;
      option.textContent = status;
      licenseStatusSelect.appendChild(option);
    });

    licenseTypeSelect.innerHTML = "";
    licenseTypes.forEach(type => {
      const option = document.createElement("option");
      option.value = type;
      option.textContent = type;
      licenseTypeSelect.appendChild(option);
    });

    licenseStatusSelect.value = "Active";
    licenseTypeSelect.value = "Premium";
  }

  function fillForm(license) {
    licenseIdInput.value = license.id;
    licenseKeyInput.value = license.key;
    licenseEmailInput.value = license.email;
    licenseTypeSelect.value = license.type;
    licenseStatusSelect.value = license.status;

    if (license.expiresAt) {
      const expires = new Date(license.expiresAt);
      const localValue = new Date(expires.getTime() - expires.getTimezoneOffset() * 60000)
        .toISOString()
        .slice(0, 16);
      licenseExpiresInput.value = localValue;
      expirationEnabledCheckbox.checked = true;
      licenseExpiresInput.disabled = false;
    } else {
      expirationEnabledCheckbox.checked = false;
      licenseExpiresInput.disabled = true;
      licenseExpiresInput.value = "";
    }

    licenseNotesInput.value = license.notes ?? "";
    formTitle.textContent = `Editar licença ${license.key}`;
    saveButton.textContent = "Atualizar";
  }

  function renderLicenses(licenses) {
    licensesBody.innerHTML = "";
    licenses.forEach(license => {
      const tr = document.createElement("tr");
      tr.innerHTML = `
        <td>${escapeHtml(license.key)}</td>
        <td>${escapeHtml(license.email)}</td>
        <td>${escapeHtml(license.type)}</td>
        <td><span class="badge ${license.status}">${license.status}</span></td>
        <td>${formatDate(license.createdAt)}</td>
        <td>${license.expiresAt ? formatDate(license.expiresAt) : "&mdash;"}</td>
        <td>${license.notes ? escapeHtml(license.notes) : "&mdash;"}</td>
        <td>
          <div class="actions" data-id="${license.id}">
            <button data-action="edit">Editar</button>
            <button data-action="activate">Ativar</button>
            <button data-action="deactivate">Inativar</button>
            <button data-action="ban">Banir</button>
            <button data-action="delete" class="danger">Excluir</button>
          </div>
        </td>`;
      licensesBody.appendChild(tr);
    });
  }

  function renderAudit(entries) {
    auditList.innerHTML = "";
    if (!entries.length) {
      const empty = document.createElement("li");
      empty.textContent = "Nenhum evento recente.";
      empty.style.color = "var(--muted)";
      auditList.appendChild(empty);
      return;
    }

    entries.forEach(entry => {
      const li = document.createElement("li");
      li.className = "audit-item";
      li.innerHTML = `
        <time>${formatDate(entry.timestamp)}</time>
        <strong>${escapeHtml(entry.action)}</strong><br>
        Usuário: ${escapeHtml(entry.performedBy)}<br>
        ${entry.licenseKey ? `Licença: ${escapeHtml(entry.licenseKey)}<br>` : ""}
        ${entry.details ? `<span style="color: var(--muted);">${escapeHtml(entry.details)}</span>` : ""}
      `;
      auditList.appendChild(li);
    });
  }

  function escapeHtml(value) {
    return String(value).replace(/[&<>"']/g, char => {
      const map = { "&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;", "'": "&#39;" };
      return map[char] ?? char;
    });
  }

  function formatDate(value) {
    if (!value) return "";
    const date = new Date(value);
    return date.toLocaleString();
  }

  async function refreshLicenses() {
    const licenses = await apiFetch("/admin/api/licenses");
    currentLicenses = Array.isArray(licenses) ? licenses : [];
    renderLicenses(currentLicenses);
  }

  async function refreshAudit() {
    const entries = await apiFetch("/admin/api/audit?limit=50");
    renderAudit(Array.isArray(entries) ? entries : []);
  }

  async function refreshData() {
    await Promise.all([refreshLicenses(), refreshAudit()]);
  }

  async function submitLicense(event) {
    event.preventDefault();
    const id = licenseIdInput.value;
    const payload = {
      key: licenseKeyInput.value.trim(),
      email: licenseEmailInput.value.trim(),
      type: licenseTypeSelect.value,
      status: licenseStatusSelect.value,
      expiresAt:
        expirationEnabledCheckbox.checked && licenseExpiresInput.value
          ? new Date(licenseExpiresInput.value).toISOString()
          : null,
      notes: licenseNotesInput.value.trim()
    };

    try {
      if (!payload.key || !payload.email) {
        throw new Error("Informe a chave e o e-mail da licença.");
      }

      if (id) {
        await apiFetch(`/admin/api/licenses/${id}`, {
          method: "PUT",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(payload)
        });
        showMessage("Licença atualizada com sucesso.");
      } else {
        await apiFetch("/admin/api/licenses", {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(payload)
        });
        showMessage("Licença criada com sucesso.");
      }

      resetLicenseForm();
      await refreshData();
    } catch (error) {
      showMessage(error.message, "error");
    }
  }

  async function changeStatus(id, status) {
    try {
      await apiFetch(`/admin/api/licenses/${id}/status`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ status })
      });
      showMessage(`Status alterado para ${status}.`);
      await refreshLicenses();
    } catch (error) {
      showMessage(error.message, "error");
    }
  }

  async function deleteLicense(id) {
    if (!confirm("Confirma a exclusão desta licença?")) {
      return;
    }

    try {
      await apiFetch(`/admin/api/licenses/${id}`, { method: "DELETE" });
      showMessage("Licença removida.");
      await refreshData();
    } catch (error) {
      showMessage(error.message, "error");
    }
  }

  loginForm.addEventListener("submit", login);
  logoutButton.addEventListener("click", logout);
  licenseForm.addEventListener("submit", submitLicense);
  refreshButton.addEventListener("click", refreshData);
  refreshAuditButton.addEventListener("click", refreshAudit);
  resetButton.addEventListener("click", resetLicenseForm);
  expirationEnabledCheckbox.addEventListener("change", () => {
    licenseExpiresInput.disabled = !expirationEnabledCheckbox.checked;
    if (!expirationEnabledCheckbox.checked) {
      licenseExpiresInput.value = "";
    }
  });

  licensesBody.addEventListener("click", event => {
    const button = event.target.closest("button");
    if (!button) return;

    const container = button.closest(".actions");
    const id = container?.dataset.id;
    if (!id) return;

    const license = currentLicenses.find(item => item.id === id);
    if (!license) return;

    switch (button.dataset.action) {
      case "edit":
        fillForm(license);
        break;
      case "activate":
        changeStatus(id, "Active");
        break;
      case "deactivate":
        changeStatus(id, "Inactive");
        break;
      case "ban":
        changeStatus(id, "Banned");
        break;
      case "delete":
        deleteLicense(id);
        break;
      default:
        break;
    }
  });

  document.addEventListener("DOMContentLoaded", async () => {
    populateSelectOptions();
    resetLicenseForm();

    if (authToken) {
      try {
        await refreshData();
        toggleDashboard(true);
        showMessage("Sessão restaurada.");
      } catch (error) {
        handleUnauthorized();
      }
    }
  });
})();
