describe('Password recovery test with server', () => {
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

    it('sends the email to recover the password and it navigates to the link of the new password creation to create it.' +
        'It tries to login with the old password and then it logs in successfully with the new one.', () => {
        cy.server();
        cy.route('POST', `${Cypress.env('EXTERNAL_API')}/api/PasswordRecovery`).as('requestPasswordRecovery');
        cy.visit('/passwordRecovery');
        cy.url().should('include', 'passwordRecovery');
        cy.get('[data-cy=email]').clear().type(credentials.email);
        cy.get('[data-cy=submit]').click();
        cy.wait('@requestPasswordRecovery');
        cy.get('[data-cy=success-div]').should('be.visible');

        cy.getEmailText().then(url => {
            cy.cleanInbox();
            const urlPasswordRecoveryEndpoint = url.split(Cypress.env('BASE_URL'))[1];

            cy.route({
                method: 'POST',
                url: `${Cypress.env('EXTERNAL_API')}/api/PasswordRecovery/reset`,
            }).as('resetPassword');

            cy.visit(urlPasswordRecoveryEndpoint);
            cy.get('[data-cy=password]').clear().type(credentials.newPassword);
            cy.get('[data-cy=confirmedPassword]').clear().type(credentials.newPassword);
            cy.get('[data-cy=submit]').click();
            cy.wait('@resetPassword');
            cy.url().should('include', '/login');

            cy.route('POST', `${Cypress.env('EXTERNAL_API')}/api/login`).as('login');
            cy.get('[data-cy=email]').clear().type(credentials.email);
            cy.get('[data-cy=password]').clear().type(credentials.password);
            cy.get('[data-cy=submit]').click();
            cy.wait('@login');
            cy.get('[data-cy=error-div]').should('be.visible');
            cy.url().should('include', 'login');

            cy.get('[data-cy=email]').clear().type(credentials.email);
            cy.get('[data-cy=password]').clear().type(credentials.newPassword);
            cy.get('[data-cy=submit]').click();
            cy.wait('@login');
            cy.url().should('include', 'home');
        });
    });
});