// ***********************************************
// This example commands.js shows you how to
// create various custom commands and overwrite
// existing commands.
//
// For more comprehensive examples of custom
// commands please read more here:
// https://on.cypress.io/custom-commands
// ***********************************************
//
//
// -- This is a parent command --
// Cypress.Commands.add("login", (email, password) => { ... })
//
//
// -- This is a child command --
// Cypress.Commands.add("drag", { prevSubject: 'element'}, (subject, options) => { ... })
//
//
// -- This is a dual command --
// Cypress.Commands.add("dismiss", { prevSubject: 'optional'}, (subject, options) => { ... })
//
//
// -- This is will overwrite an existing command --
// Cypress.Commands.overwrite("visit", (originalFn, url, options) => { ... })
Cypress.Commands.add("store", () => cy.window().its('appStore'));
Cypress.Commands.add("login", credentials => cy.store().dispatch("login", credentials));
Cypress.Commands.add("register", credentials => cy.store().dispatch("register", credentials));
Cypress.Commands.add("deleteMyself", () => cy.store().dispatch("deleteMyself"));
Cypress.Commands.add("removeUser", (email) => cy.request({
  url: `${Cypress.env('EXTERNAL_API')}/api/cypress/deleteUser/${email}`, 
  headers: {CYPRESS_TOKEN: Cypress.env('CYPRESS_TOKEN')}
}));