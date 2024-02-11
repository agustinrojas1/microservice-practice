FROM rabbitmq:3-management

COPY appsettings.Development.json ./appsettings.Development

EXPOSE 5672 15672

CMD ["rabbitmq-server", "-f", "/appsettings.Development"]