using SharedKernel;

namespace Portfolio.Domain.Auth;

public static class RefreshTokenErrors
{
    public static Error ErrorInternal =>
        Error.Failure(
            "Auth.RefreshToken.Internal",
            "Ocorreu um erro inesperado ao processar a renovação do token.");

    public static Error InvalidRefreshToken =>
        Error.Failure(
            "Auth.InvalidRefreshToken",
            "O token de atualização fornecido é inválido, expirou ou já foi utilizado.");

    public static Error TokenNotFound =>
        Error.NotFound(
            "Auth.RefreshTokenNotFound",
            "Sessão não encontrada ou já encerrada.");

    public static Error LifetimeReached =>
        Error.Failure(
            "Auth.MaxLifetimeReached",
            "A sessão atingiu o tempo máximo de duração permitido e exige um novo login.");
}