/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   syntax_errors.c                                    :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: caide-so <caide-so@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/05/02 14:29:45 by caide-so          #+#    #+#             */
/*   Updated: 2025/05/20 21:41:24 by caide-so         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/minishell.h"

void	report_unexpected_quotes(const char token_value)
{
	char	*str;

	str = "minishell: syntax error near unexpected token `";
	write(STDERR_FILENO, str, ft_strlen(str));
	write(STDERR_FILENO, &token_value, 1);
	write(STDERR_FILENO, "'\n", 2);
}

int	report_unexpected(const char *token_value)
{
	char	*str;

	str = "minishell: syntax error near unexpected token `";
	write(STDERR_FILENO, str, ft_strlen(str));
	write(STDERR_FILENO, token_value, ft_strlen(token_value));
	write(STDERR_FILENO, "'\n", 2);
	return (0);
}
