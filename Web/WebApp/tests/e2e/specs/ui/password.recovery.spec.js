describe('Password recovery test', () => {

    const credentials = {
        name: "Barney Ullrich",
        email: "Rodrick_Russel81@yahoo.com",
        password: "OAsudlZIUTckj",
        newPassword: "oaskAMFK4562"
    };

    before(() => {
        cy.removeUser(credentials.email);
        cy.register(credentials);
    });

    after(() => {
        cy.removeUser(credentials.email);
    });

    it('shows a success message when the email was sent', () => {
        cy.server();
        cy.route('POST', `${Cypress.env('EXTERNAL_API')}/api/PasswordRecovery`, "URLCALLBACK").as('requestPasswordRecovery');
        cy.visit('/passwordRecovery');
        cy.get('[data-cy=email]').clear().type(credentials.email);
        cy.get('[data-cy=submit]').click();
        cy.wait('@requestPasswordRecovery');
        cy.get('[data-cy=success-div]').should('be.visible');
    });

    it('shows an error message when the passwords are not equal', () => {
        cy.visit('/passwordRecovery/aUserId/aToken');
        cy.get('[data-cy=password]').clear().type(credentials.newPassword);
        cy.get('[data-cy=confirmedPassword]').clear().type("aDifferentNewPassword");
        cy.get('[data-cy=submit]').click();
        cy.get('[data-cy=error-div]').should('be.visible');
    });

    it('redirects to login page when the new password is created', () => {
        cy.server();
        cy.route({
            method: 'POST',
            url: `${Cypress.env('EXTERNAL_API')}/api/PasswordRecovery/reset`,
            response: {}
        }).as('resetPassword');
        cy.visit('/passwordRecovery/aUserId/aToken');
        cy.get('[data-cy=password]').clear().type(credentials.newPassword);
        cy.get('[data-cy=confirmedPassword]').clear().type(credentials.newPassword);
        cy.get('[data-cy=submit]').click();
        cy.wait('@resetPassword');
        cy.url().should('include', '/login');
    });
})
;