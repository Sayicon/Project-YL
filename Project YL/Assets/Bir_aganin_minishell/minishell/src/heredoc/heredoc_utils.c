/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   heredoc_utils.c                                    :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: caide-so <caide-so@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/05/29 04:20:19 by caide-so          #+#    #+#             */
/*   Updated: 2025/05/29 04:23:06 by caide-so         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/minishell.h"

char	*get_var_value(char *line, int *ip, t_env *env, int last_sts)
{
	char	*val;
	char	*key;
	int		j;

	if (line[*ip + 1] == '?')
	{
		val = ft_itoa(last_sts);
		*ip += 2;
		return (val);
	}
	j = *ip + 1;
	while (line[j] && (ft_isalnum(line[j]) || line[j] == '_'))
		j++;
	key = ft_substr(line, *ip + 1, j - *ip - 1);
	val = find_env_value(key, env);
	free(key);
	*ip = j;
	return (val);
}

char	*ft_strjoin_char_free(char *str, char c)
{
	size_t	len;
	char	*out;

	len = ft_strlen(str);
	out = (char *)malloc(len + 2);
	if (!out)
		exit_perror("malloc");
	ft_memcpy(out, str, len);
	out[len] = c;
	out[len + 1] = '\0';
	free(str);
	return (out);
}
