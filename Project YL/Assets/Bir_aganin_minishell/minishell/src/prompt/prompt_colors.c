/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   prompt_colors.c                                    :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: caide-so <caide-so@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/05/23 00:37:29 by caide-so          #+#    #+#             */
/*   Updated: 2025/05/23 00:41:39 by caide-so         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/minishell.h"

void	build_user_host(char *dst, size_t *i, char *user, char *host)
{
	append_str(dst, i, C_RED);
	append_str(dst, i, user);
	append_char(dst, i, '@');
	append_str(dst, i, host);
	append_str(dst, i, C_RESET);
}

void	build_cwd(char *dst, size_t *i, char *cwd)
{
	append_char(dst, i, ':');
	append_str(dst, i, C_YELLOW);
	append_str(dst, i, cwd);
	append_str(dst, i, C_RESET);
}

void	append_str(char *dst, size_t *i, const char *src)
{
	size_t	len;

	len = ft_strlen(src);
	ft_memcpy(dst + *i, src, len);
	*i += len;
}

void	append_char(char *dst, size_t *i, char c)
{
	dst[(*i)++] = c;
}
