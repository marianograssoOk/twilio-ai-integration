# twilio-ai-integration

Este proyecto contiene código para una aplicación serverless que puedes desplegar con el SAM CLI. Incluye:

- `src` - Código fuente de la función Lambda.
- `events` - Ejemplos de eventos de invocación.
- `test` - Pruebas unitarias.
- `template.yaml` - Plantilla para definir recursos de AWS.

## Requisitos

- [SAM CLI](https://docs.aws.amazon.com/serverless-application-model/latest/developerguide/serverless-sam-cli-install.html)
- [Docker](https://hub.docker.com/search/?type=edition&offering=community)
- [.NET Core](https://www.microsoft.com/net/download)

## Configuración de Entorno

Puedes usar un IDE con AWS Toolkit para desarrollar, depurar y desplegar la aplicación. IDEs soportados: [VS Code](https://docs.aws.amazon.com/toolkit-for-vscode/latest/userguide/welcome.html), IntelliJ, PyCharm, entre otros.

## Despliegue

Utiliza el SAM CLI para construir y desplegar la aplicación. Docker es necesario para ejecutar funciones Lambda localmente.
